using Skalm.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStateInitializing : GameStateBase
    {

        // CONSTRUCTOR I
        public GameStateInitializing(GameManager gameManager) : base(gameManager) { }

        // ENTER STATE
        public override void Enter()
        {
            displayManager.Printer.PrintCenteredInWindow("Loading SKÄLM", displayManager.WindowInfo.WindowHeight / 2);
        }

        // EXIT STATE
        public override void Exit()
        {
            displayManager.Eraser.EraseAll(); 
            displayManager.Printer.PrintCenteredInWindow("Loaded", displayManager.WindowInfo.WindowHeight / 2);
            Thread.Sleep(500);
            displayManager.Eraser.EraseAll();
        }

        // UPDATE LOGIC
        public override void UpdateDisplay() {}

        // UPDATE DISPLAY
        public override void UpdateLogic() {}
    }
}
