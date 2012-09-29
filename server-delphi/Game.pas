unit Game;

interface uses
  System.types, UiTypes, Messages, SysUtils, Classes, IdContext, Contnrs, Math, IdIOHandler, FMX.Types, FMX.Forms;

type
  TScreenObj = class
    Loc, Size, Speed: TPointF;
    Color: DWord;
    procedure Paint(Canvas: TCanvas); virtual;
    procedure Move(delta: Single); virtual;
  end;

  TBall = class(TScreenObj)
    RotationSpeed, CurrentAngle: Single;
    procedure Paint(Canvas: TCanvas); override;
    procedure Move(delta: Single); override;
    procedure Reset;
    procedure Restart(Team: Integer = 1);
  end;

  TClientData = packed record
    AccX, AccY, AccZ, timeDelta: Single
  end;

  TPlayer = class(TScreenObj)
  private
    cnt: Integer;
    lastDT: TDateTime;
    Sensor: TClientData;
    Ping: single;
    FLastCmd: Integer;
    FLastData:TBytes;
    FLastTx: TDateTime;
    function Intersects(AShape: TScreenObj; out Where: Single): Boolean;
    procedure TryToHitTheBall(Ball: TBall);
  public
    NetContext: TIdContext;
    Team: Integer;
    Name: string;
    constructor Create(Pattern: TPointF);
    destructor Destroy; override;
    procedure Move(delta: Single); override;
    procedure Paint(Canvas: TCanvas); override;
    procedure ListenToClient(); // threaded !
  end;

  TGame = class(TObjectList)
  const
    MinPlayers = 1;
  private
    class var FieldSize: TPointF;
    FScore: array[0..1] of Integer;
    FGoalShowing: Boolean;
    GoalTime: TDateTime;
    LastScorer: Integer;
    procedure Rearrange;
    function GetItem(Index: Integer): TPlayer;
    procedure ShowGoal(Team: Integer);
  protected
    procedure Notify(Ptr: Pointer; Action: TListNotification); override;
  public
    PlayerPattern: TPointF;
    Ball: TBall;
    constructor Create;
    destructor Destroy; override;
    procedure Resize(w,h: Integer);
    procedure CreatePlayer(NetContext: TIdContext);
    function FindByContext(NetContext: TIdContext): TPlayer;
    procedure Move(delta: Single);
    procedure Paint(Canvas: TCanvas);
    function GetScore: string;
    property Players[Index: Integer]: TPlayer read GetItem; default;
  end;

implementation

{ TScreenObj }

procedure TScreenObj.Move(delta: Single);
begin
  Loc.x := Loc.x + Speed.x{-AvgValue}*delta;
  Loc.y := Loc.y + Speed.y{-AvgValue}*delta;
end;

procedure TScreenObj.Paint(Canvas: TCanvas);
begin
  Canvas.Fill.Color := Color;
  Canvas.FillEllipse(tRectf.Create(Loc.x-Size.x, Loc.y-Size.y,
    Loc.x+Size.x, Loc.y+Size.y), 1);
  Canvas.DrawEllipse(tRectf.Create(Loc.x-Size.x, Loc.y-Size.y,
    Loc.x+Size.x, Loc.y+Size.y), 1);
end;

{ TBall }

procedure TBall.Move(delta: Single);
var
  spd: Single;
  spdAngle: Single;
  HitRotationBonus: Boolean;
begin
  inherited Move(delta);
  HitRotationBonus := false;
  if Loc.y <= Size.y then begin
    Loc.y := Size.y;
    Speed.y := -Speed.y;
    HitRotationBonus := true;
  end;
  if Loc.y >= TGame.FieldSize.y-Size.y then begin
    Loc.y := TGame.FieldSize.y-Size.y;
    Speed.y := -Speed.y;
    HitRotationBonus := true;
  end;
  if TGame.MinPlayers < 2 then begin
    if Loc.x >= TGame.FieldSize.x-Size.x then begin
      Loc.x := TGame.FieldSize.x-Size.x-1;
      Speed.x := -Speed.x;
      HitRotationBonus := true;
    end;
  end;
  CurrentAngle := CurrentAngle + RotationSpeed*delta;
  spdAngle := ArcTan2(Speed.Y, Speed.X);
  if HitRotationBonus then begin
    spdAngle := delta*RotationSpeed/10 + spdAngle;
    RotationSpeed := RotationSpeed/10;
  end else begin
    spdAngle := delta*RotationSpeed/100 + spdAngle;
    RotationSpeed := RotationSpeed*Exp(-delta/10);
  end;
  spd := Sqrt(Sqr(Speed.X) + Sqr(Speed.Y));
  Speed.X := cos(spdAngle)*spd;
  Speed.Y := sin(spdAngle)*spd;
end;

procedure TBall.Paint(Canvas: TCanvas);
var
  x, y: Single;
begin
  inherited;
  x := Loc.x+sin(CurrentAngle)*Size.X*0.7;
  y := Loc.y+cos(CurrentAngle)*Size.Y*0.7;
  Canvas.Fill.Color := TAlphaColorRec.Black;
  Canvas.FillEllipse(tRectf.Create(x-2, y-2, x+2, y+2), 1);
end;

procedure TBall.Reset;
begin
  Speed.x := 0;
  Speed.y := 0;
  Loc.x := TGame.FieldSize.x /2;
  Loc.y := TGame.FieldSize.y /2;
  RotationSpeed := 0;
end;

procedure TBall.Restart(Team: Integer);
begin
  Speed.x := (1-Team*2)*TGame.FieldSize.x / 04;
  Speed.y := 0;
end;

{ TPlayerInfo }

constructor TPlayer.Create(Pattern: TPointF);
begin
  Color := TAlphaColorRec.Pink;
  Size.x := Pattern.X/2;
  Size.y := Pattern.Y/2;
  Loc.y := TGame.FieldSize.y/2;
end;

destructor TPlayer.Destroy;
begin
  inherited;
end;

function TPlayer.Intersects(AShape: TScreenObj; out Where: Single): Boolean;
begin
  Result := (AShape.Loc.y + AShape.Size.y >= Loc.y - Size.y)
    and (AShape.Loc.y - AShape.Size.y <= Loc.y + Size.y);
  Result := Result and
    (AShape.Loc.x + AShape.Size.x >= Loc.x - Size.x)
      and (AShape.Loc.x - AShape.Size.x <= Loc.x + Size.x);
  if Result then
    Where := (AShape.Loc.y - Loc.y)/Size.y;
end;

procedure TPlayer.ListenToClient;
var
  io: TIdIOHandler;
  cmd: Integer;
const
  OsColors: array[0..3] of DWord = (TAlphaColorRec.Black, TAlphaColorRec.Yellow, TAlphaColorRec.Blue,
    TAlphaColorRec.Silver);

  procedure onExit;
  begin
    FLastCmd := cmd;
    if lastDT = 0 then
      lastDT := now
    else
      inc(cnt);
    if (Now-lastDT>1/86400) then begin
      Ping := (Now-lastDT)*86400000/cnt;
      lastDT := Now;
      cnt := 0;
    end;
  end;

begin
  io := NetContext.Connection.IOHandler;
  io.CheckForDataOnSource(1);
  while not io.InputBufferIsEmpty do begin
    cmd := io.ReadLongInt();
    FLastData := nil;
    case cmd of
    0: begin
      io.ReadBytes(FLastData, 16);
      system.Move(FLastData[0], Sensor, 16);
      Move(Sensor.timeDelta);
    end;
    1: Color := OsColors[io.ReadLongInt];
    2: begin
      SetLength(Name, io.ReadLongInt);
      Name := io.ReadString(length(Name)*2, TEncoding.Unicode);
    end;
    else
      io.ReadBytes(FLastData, io.InputBuffer.Size);
    end;
    onExit;
  end;
end;

procedure TPlayer.Move(delta: Single);
begin
  Speed.y := -Sensor.AccY;
  inherited Move(delta);
  Loc.y := Max(Size.y*0.5, Loc.y);
  Loc.y := Min(TGame.FieldSize.y-Size.y*0.5, Loc.y);
  FLastTx := Now;
end;

procedure TPlayer.Paint(Canvas: TCanvas);
begin
  inherited Paint(Canvas);
  Canvas.Font.Size := 9;
  Canvas.Fill.Color := TAlphaColorRec.White;
  Canvas.FillText(TRectf.Create(Loc.x-Size.x, Loc.y-Size.y-8, Loc.x+Size.x, Loc.y-Size.y),
    Name, false, 1, [], TTextAlign.taCenter);
  if Color = TAlphaColorRec.Black then
    Canvas.Fill.Color := TAlphaColorRec.Yellow
  else
    Canvas.Fill.Color := TAlphaColorRec.Black;
  if Ping <> 0 then
    Canvas.FillText(TRectf.Create(Loc.x-Size.x, Loc.y-8, Loc.x+Size.x, Loc.y),
      Format('%2.0f ms', [Ping]), false, 1, [], TTextAlign.taCenter);
{  Canvas.FillText(TRectf.Create(Loc.x-Size.x, Loc.y+8, Loc.x+Size.x, Loc.y+16),
    FormatDateTime('zzz', FLastTx), false, 1, [], TTextAlign.taCenter);}
end;

procedure TPlayer.TryToHitTheBall(Ball: TBall);
var
  sp, wh: Single;
begin
  if not Intersects(Ball, wh) then
    Exit;
  if (Team = 0) xor (Ball.Speed.x>0) then begin
    sp := (1.1-abs(wh)/10) * sqrt(sqr(Ball.Speed.x)+Sqr(Ball.Speed.y));
    Ball.Speed.x := - sign(Ball.Speed.x) * sp * cos(wh);
    Ball.Speed.y := sin(wh) * sp;
  end;
  Ball.RotationSpeed := -Sensor.AccX*sqrt(sqr(Ball.Speed.X)+sqr(Ball.Speed.Y))/10;
end;

{ TPlayers }

constructor TGame.Create;
begin
  Ball := TBall.Create;
  Ball.Color := TAlphaColorRec.White;
end;

procedure TGame.CreatePlayer(NetContext: TIdContext);
var
  pi: TPlayer;
begin
  pi := TPlayer.Create(PlayerPattern);
  pi.NetContext := NetContext;
  Add(pi);
  if Count >= MinPlayers then
    Ball.Restart;
end;

destructor TGame.Destroy;
begin
  inherited;
  FreeAndNil(Ball);
end;

function TGame.FindByContext(NetContext: TIdContext): TPlayer;
var
  i: Integer;
begin
  Result := nil;
  for i := 0 to Count-1 do
    if Players[i].NetContext = NetContext then
      Result := Players[i];
end;

function TGame.GetItem(Index: Integer): TPlayer;
begin
  Result := TPlayer(inherited GetItem(index))
end;

function TGame.GetScore: string;
begin
  Result := Format('%d:%d', [fscore[0], fscore[1]]);
end;

procedure TGame.ShowGoal(Team: Integer);
begin
  Inc(FScore[1-team]);
  GoalTime := Now;
  FGoalShowing := true;
  LastScorer := Team;
end;

procedure TGame.Move(delta: Single);
var
  i: Integer;
begin
  if FGoalShowing then begin
      if round(Now*86400000) mod 1000 < 500 then
        Ball.Color := TAlphaColorRec.Red
      else
        Ball.Color := TAlphaColorRec.White;
    if Now - GoalTime > 3/86400 then begin
      FGoalShowing := false;
      Ball.Color := TAlphaColorRec.White;
      Ball.Reset;
      if Count >= MinPlayers then
        Ball.Restart(LastScorer);
    end else
      Exit;
  end;

  Ball.Move(delta);

  if Ball.Loc.x <= Ball.Size.x then
    ShowGoal(0);
  if Ball.Loc.x >= FieldSize.x-Ball.Size.x then
    ShowGoal(1);

  for i := 0 to Count-1 do
    Players[i].TryToHitTheBall(Ball);
end;

procedure TGame.Notify(Ptr: Pointer; Action: TListNotification);
begin
  inherited;
  Rearrange;
  if Count < 2 then
    Ball.Reset;
end;

procedure TGame.Paint(Canvas: TCanvas);
var
  i: Integer;
begin
  Canvas.Clear(TAlphaColorRec.Green);
  Canvas.Fill.Color := TAlphaColorRec.White;
  Canvas.FillRect(TRectf.Create(4, 4, FieldSize.x-4, 8), 0, 0, [], 1);
  Canvas.FillRect(TRectf.Create(4, 4, 8, round(FieldSize.y-4)), 0, 0, [], 1);
  Canvas.FillRect(TRectf.Create(round(FieldSize.x-8), 4, round(FieldSize.x-4), round(FieldSize.y-4)), 0, 0, [], 1);
  Canvas.FillRect(TRectf.Create(4, round(FieldSize.y-8), round(FieldSize.x-4), round(FieldSize.y-4)), 0, 0, [], 1);
  for i := 0 to Count-1 do
    Players[i].Paint(Canvas);
  Ball.Paint(Canvas);
  Canvas.Fill.Color := TAlphaColorRec.Green;
end;

procedure TGame.Rearrange;
var
  i: Integer;
begin
  for i := 0 to Count-1 do begin
    Players[i].Team := i mod 2;
    if Players[i].Team = 0 then
      Players[i].Loc.x := PlayerPattern.X*(i+1)
    else
      Players[i].Loc.x := FieldSize.x - PlayerPattern.X*(i+1);
  end;
  if Ball <> nil then
    Ball.Reset;
  FScore[0] := 0;
  FScore[1] := 0;
end;

procedure TGame.Resize(w, h: Integer);
begin
  FieldSize.x := w;
  FieldSize.y := h;
  Ball.Size.x := FieldSize.x/80;
  Ball.Size.y := Ball.Size.x;
end;

end.
