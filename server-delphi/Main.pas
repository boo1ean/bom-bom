unit Main;

interface uses
  Windows, System.SysUtils, System.Types, System.UITypes, System.Rtti, System.Classes,
  System.Variants, FMX.Types, FMX.Controls, FMX.Forms, FMX.Dialogs, Contnrs,
  Game, IdContext, IdBaseComponent, IdComponent, IdCustomTCPServer,
  IdTCPServer, IdThreadComponent;

type
  TfMain = class(TForm)
    Timer1: TTimer;
    IdTCPServer1: TIdTCPServer;
    procedure FormPaint(Sender: TObject; Canvas: TCanvas;
      const ARect: TRectF);
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure Timer1Timer(Sender: TObject);
    procedure FormKeyDown(Sender: TObject; var Key: Word;
      var KeyChar: Char; Shift: TShiftState);
    procedure FormResize(Sender: TObject);
    procedure IdTCPServer1Connect(AContext: TIdContext);
    procedure IdTCPServer1Disconnect(AContext: TIdContext);
    procedure IdTCPServer1Execute(AContext: TIdContext);
  private
    Game: TGame;
    LastDT: TDateTime;
    Fps: Single;
    FpsCnt: Integer;
  public
    { Public declarations }
  end;

var
  fMain: TfMain;

implementation

{$R *.fmx}

procedure TfMain.FormCreate(Sender: TObject);
begin
  SetThreadAffinityMask(GetCurrentThread, 2);
  ShowCursor(false);
  Game := TGame.Create;
  OnResize(self);
end;

procedure TfMain.FormDestroy(Sender: TObject);
begin
  Game.Free;
end;

procedure TfMain.FormKeyDown(Sender: TObject; var Key: Word;
  var KeyChar: Char; Shift: TShiftState);
begin
  case Key of
  vkEscape:
    Application.Terminate;
  end;
end;

procedure TfMain.FormPaint(Sender: TObject; Canvas: TCanvas;
  const ARect: TRectF);
var
  i: Integer;
begin
  if Application.Terminated then begin
    Canvas.Clear(TAlphaColorRec.Black);
    Exit;
  end;
  for I := 1 to 1 do begin
    Game.Paint(Canvas);
    Canvas.Font.Style := [];
    Canvas.Font.Size := 16;
    Canvas.Fill.Color := TAlphaColorRec.White;
    if Fps <> 0 then
      Canvas.FillText(TRectF.Create(0, 20, Width, 40), Format('%2.0f fps', [fps]),
        false, 1, [], TTextAlign.taCenter);
    Canvas.Font.Style := [TFontStyle.fsBold];
    Canvas.Font.Size := 60;
    Canvas.FillText(TRectF.Create(0, Height-90, Width, Height), Game.GetScore,
      false, 1, [], TTextAlign.taCenter);
    if LastDT = 0 then
      LastDT := Now
    else
      Inc(FpsCnt);
    if (FpsCnt = 10) and (Now > LastDT) then begin
      fps := fpscnt/((now-lastDt)*86400);
      FpsCnt := 0;
      LastDT := Now;
    end;
  end;
end;

procedure TfMain.FormResize(Sender: TObject);
begin
  with Game.PlayerPattern do begin
    x := ClientWidth/50;
    y := ClientHeight/7;
  end;
  Game.Resize(ClientWidth, ClientHeight);
  Game.Ball.Reset;
end;

procedure TfMain.IdTCPServer1Connect(AContext: TIdContext);
begin
  Game.CreatePlayer(AContext);
end;

procedure TfMain.IdTCPServer1Disconnect(AContext: TIdContext);
begin
  if Game = nil then
    Exit;
  Game.Remove(Game.FindByContext(AContext));
end;

procedure TfMain.IdTCPServer1Execute(AContext: TIdContext);
var
  Player: TPlayer;
begin
  if Application.Terminated then
    Exit;
  Player := Game.FindByContext(AContext);
  if Player = nil then
    Exit;
  Player.ListenToClient;
end;

procedure TfMain.Timer1Timer(Sender: TObject);
const
  lastDT: TDateTime = 0;
var
  delta: Single;
begin
  if Application.Terminated then begin
    Timer1.Enabled := false;
    Exit;
  end;
  if lastDT = 0 then begin
    lastDT := Now;
    Exit;
  end;
  delta := (Now-lastDT)*86400;
  if delta = 0 then
    Exit;
  lastDT := Now;
  Game.Move(delta);
  Invalidate;
end;

end.
