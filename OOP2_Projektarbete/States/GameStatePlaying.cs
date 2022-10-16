using Skalm.Display;
using Skalm.Input;
using Skalm.Map;

using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.States
{
    internal class GameStatePlaying : GameStateBase
    {
        private readonly MapManager mapManager;
        private readonly MapPrinter mapPrinter;

        // CONSTRUCTOR I
        public GameStatePlaying(GameManager gameManager) : base(gameManager)
        {
            mapManager = gameManager.MapManager;
            mapPrinter = mapManager.mapPrinter;
        }

        #region State Machine Basics

        // ENTER GAME PLAYING STATE
        public override void Enter()
        {
            // INPUT EVENT SUBSCRIBE
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            if (gameManager.NewGame)
            {
                displayManager.printer.PrintCenteredInWindow("ENTERING SKÄLM", displayManager.windowInfo.WindowHeight / 2);
                Thread.Sleep(500);
                mapManager.ResetMap();

                // CREATE PLAYER
                gameManager.CreatePlayer();

                // CREATE OTHER ACTORS


                // CREATE ITEMS


                // CREATE MAP
                mapManager.mapGenerator.CreateMap();
                mapManager.AddActorsToMap();
            }

            // DRAWING HUD & MAP
            displayManager.eraser.EraseAll();
            displayManager.DisplayHUD();
            mapManager.mapPrinter.DrawMap();


            if (gameManager.NewGame)
                displayManager.pixelGridController.DisplayMessage("Welcome traveller.");
            else
                displayManager.pixelGridController.DisplayMessage("Welcome back.");

            gameManager.player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
            soundManager.PlayMusic(soundManager.Tracks.Find(song => song.soundName == "Thunder Dreams"));
        }

        // EXIT GAME PLAYING STATE
        public override void Exit()
        {
            // SAVING STATE
            gameManager.NewGame = false;
            gameManager.player.playerStateMachine.ChangeState(PlayerStates.PlayerStateIdle);
            displayManager.eraser.EraseAll();

            // INPUT EVENT UNSUBSCRIBE
            inputManager.OnInputMove -= MoveInput;
            inputManager.OnInputCommand -= CommandInput;
        }

        // UPDATE STATE LOGIC
        public override void UpdateLogic()
        {
            foreach (var go in gameManager.MapManager.gameObjects)
            {
                go.UpdateMain();
            }
        }

        // UPDATE STATE DISPLAY
        public override void UpdateDisplay()
        {
            mapPrinter.RedrawCachedTiles();
        }

        #endregion

        #region Methods

        //METHOD MOVE INPUT
        private void MoveInput(Vector2Int direction)
        {
            gameManager.player.playerStateMachine.CurrentState.MoveInput(direction);
        }

        // METHOD COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            gameManager.player.playerStateMachine.CurrentState.CommandInput(command);
        }

        #endregion
    }
}
