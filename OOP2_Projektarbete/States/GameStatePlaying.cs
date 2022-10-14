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
        public GameManager GameManager { get; }

        private readonly InputManager inputManager;
        private readonly DisplayManager displayManager;
        private readonly SoundManager soundManager;
        private readonly MapManager mapManager;
        private readonly MapPrinter mapPrinter;

        private bool isResuming;

        // CONSTRUCTOR I
        public GameStatePlaying(GameManager gameManager)
        {
            this.GameManager = gameManager;

            inputManager = gameManager.InputManager;
            displayManager = gameManager.DisplayManager;
            soundManager = gameManager.SoundManager;
            mapManager = gameManager.MapManager;
            mapPrinter = gameManager.MapPrinter;

            isResuming = false;
        }

        #region State Machine Basics

        // ENTER GAME PLAYING STATE
        public void Enter()
        {
            //if (isResuming)
                // ONLY DO STUFF LIKE REDRAW UI
            //else
                // DO EVERYTHING

            // INPUT EVENT SUBSCRIBE
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            displayManager.printer.PrintCenteredInWindow("Starting new game", displayManager.windowInfo.WindowHeight / 2);
            Thread.Sleep(500);
            displayManager.eraser.EraseAll();
            displayManager.DisplayHUD();

            // CREATE MAP

            mapManager.mapGenerator.CreateMap();
            mapManager.AddActorsToMap();
            mapManager.mapPrinter.DrawMap();


            soundManager.PlayMusic(soundManager.Tracks.Find(song => song.soundName == "Thunder Dreams"));
            displayManager.pixelGridController.DisplayMessage("Welcome to the Land of Skälm.");
        }

        // EXIT GAME PLAYING STATE
        public void Exit()
        {
            // SAVING STATE
            isResuming = true;
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
                GameManager.stateMachine.ChangeState(GameManager.gameStates.Find(state => state is GameStatePaused)!);
        }

        #endregion
    }
}
