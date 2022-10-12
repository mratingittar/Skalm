using Skalm.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStatePlaying : IGameState
    {
        private readonly DisplayManager displayManager;

        // CONSTRUCTOR I
        public GameStatePlaying(DisplayManager displayManager)
        {
            this.displayManager = displayManager;
        }

        // ENTER GAME PLAYING STATE
        public void Enter()
        {
            // IF NEW GAME
            // ELSE IF RESUMING


            displayManager.printer.PrintCenteredInWindow("Starting new game", displayManager.windowInfo.WindowHeight / 2);
            Thread.Sleep(500);
            displayManager.eraser.EraseAll();
            displayManager.DisplayHUD();
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
    }
}
