using Skalm.Display;
using Skalm.Input;
using Skalm.Map;

using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.States
{
    internal class GameStatePlaying : IGameState
    {
        public GameManager GameManager { get; }

        private readonly InputManager inputManager;
        private readonly DisplayManager displayManager;
        private readonly SoundManager soundManager;
        private readonly MapManager mapManager;
        private readonly MapPrinter mapPrinter;

        // CONSTRUCTOR I
        public GameStatePlaying(GameManager gameManager)
        {
            this.GameManager = gameManager;

            inputManager = gameManager.InputManager;
            displayManager = gameManager.DisplayManager;
            soundManager = gameManager.SoundManager;
            mapManager = gameManager.MapManager;
            mapPrinter = mapManager.mapPrinter;
        }

        #region State Machine Basics

        // ENTER GAME PLAYING STATE
        public void Enter()
        {
            // INPUT EVENT SUBSCRIBE
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            if (GameManager.NewGame)
            {
                displayManager.printer.PrintCenteredInWindow("ENTERING SKÄLM", displayManager.windowInfo.WindowHeight / 2);
                Thread.Sleep(500);
                mapManager.ResetMap();

                // CREATE PLAYER
                GameManager.CreatePlayer();

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


            if (GameManager.NewGame)
                displayManager.pixelGridController.DisplayMessage("Welcome traveller.");
            else
                displayManager.pixelGridController.DisplayMessage("Welcome back.");


            soundManager.PlayMusic(soundManager.Tracks.Find(song => song.soundName == "Thunder Dreams"));
        }

        // EXIT GAME PLAYING STATE
        public void Exit()
        {
            // SAVING STATE
            GameManager.NewGame = false;
            displayManager.eraser.EraseAll();

            // INPUT EVENT UNSUBSCRIBE
            inputManager.OnInputMove -= MoveInput;
            inputManager.OnInputCommand -= CommandInput;
        }

        // UPDATE STATE LOGIC
        public void UpdateLogic()
        {
            // _gameManager.UpdateGameLoop/(;
        }

        // UPDATE STATE DISPLAY
        public void UpdateDisplay()
        {
            mapPrinter.RedrawCachedTiles();
        }

        #endregion

        #region Methods

        //METHOD MOVE INPUT
        private void MoveInput(Vector2Int direction)
        {
            GameManager.player.Move(direction);
        }

        // METHOD COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            if (command == InputCommands.Cancel)
                GameManager.stateMachine.ChangeState(GameManager.gameStates.Find(state => state is GameStatePaused)!);
        }

        #endregion
    }
}
