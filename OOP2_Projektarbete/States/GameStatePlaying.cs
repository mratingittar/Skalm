using Skalm.Display;
using Skalm.Input;
using Skalm.Map;

using Skalm.Sounds;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStatePlaying : IGameState
    {
        public GameManager gameManager { get; }

        private readonly InputManager inputManager;
        private readonly DisplayManager displayManager;
        private readonly SoundManager soundManager;
        private readonly MapManager mapManager;
        private readonly MapPrinter mapPrinter;

        // CONSTRUCTOR I
        public GameStatePlaying(GameManager gameManager)
        {
            this.gameManager = gameManager;

            inputManager = gameManager.inputManager;
            displayManager = gameManager.displayManager;
            soundManager = gameManager.soundManager;
            mapManager = gameManager.mapManager;
            mapPrinter = gameManager.mapPrinter;
        }

        #region State Machine Basics

        // ENTER GAME PLAYING STATE
        public void Enter()
        {
            // IF NEW GAME
            // ELSE IF RESUMING

            // INPUT EVENT SUBSCRIBE
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            displayManager.printer.PrintCenteredInWindow("Starting new game", displayManager.windowInfo.WindowHeight / 2);
            Thread.Sleep(500);
            displayManager.eraser.EraseAll();
            displayManager.DisplayHUD();

            // CREATE MAP
            mapManager.CreateMap();
            mapPrinter.RedrawMap();

            soundManager.PlayMusic(soundManager.Tracks.Find(song => song.soundName == "Thunder Dreams"));
            displayManager.pixelGridController.DisplayMessage("Welcome to the Land of Skälm.");
        }

        // EXIT GAME PLAYING STATE
        public void Exit()
        {
            // SAVING STATE
            displayManager.eraser.EraseAll();
        }

        // UPDATE STATE LOGIC
        public void UpdateLogic()
        {
            // _gameManager.UpdateGameLoop/(;
        }

        // UPDATE STATE DISPLAY
        public void UpdateDisplay()
        {
            // _displayManager.UpdateDisplayWindows();
        }

        #endregion

        #region Methods

        //METHOD MOVE INPUT
        private void MoveInput(Vector2Int direction)
        {

        }

        // METHOD COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            if (command == InputCommands.Cancel)
                gameManager.stateMachine.ChangeState(gameManager.gameStates.Find(state => state is GameStatePaused)!);
        }

        #endregion
    }
}
