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

        // ENTER STATE
        public void Enter()
        {
            displayManager.printer.PrintCenteredInWindow("Loading SKÄLM", displayManager.windowInfo.WindowHeight / 2);
        }

        // EXIT STATE
        public void Exit()
        {
            displayManager.eraser.EraseAll(); 
            displayManager.printer.PrintCenteredInWindow("Loaded", displayManager.windowInfo.WindowHeight / 2);
            Thread.Sleep(500);
            displayManager.eraser.EraseAll();
        }

        // UPDATE LOGIC
        public void UpdateDisplay() {}

        // UPDATE DISPLAY
        public void UpdateLogic() {}
    }
}
