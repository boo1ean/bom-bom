program HockeyServer;

uses
  FMX.Forms,
  Main in 'Main.pas' {fMain},
  Game in 'Game.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfMain, fMain);
  Application.Run;
end.
