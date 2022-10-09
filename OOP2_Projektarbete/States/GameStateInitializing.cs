using Skalm.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStateInitializing : IGameState
    {
        private DisplayManager displayManager;

        public GameStateInitializing(DisplayManager displayManager)
        {
            this.displayManager = displayManager;
        }

        public void Enter()
        {
            displayManager.printer.PrintCenteredInWindow("Loading SKÄLM", displayManager.WindowHeight / 2);
        }

        public void Exit()
        {
            displayManager.eraser.EraseAll(); 
            displayManager.printer.PrintCenteredInWindow("Loaded", displayManager.WindowHeight / 2);
            Thread.Sleep(500);
        }
    }
}
