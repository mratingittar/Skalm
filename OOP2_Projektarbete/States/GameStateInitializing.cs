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
            displayManager.printer.PrintCenteredInWindow("Loading SKÄLM", displayManager.windowInfo.WindowHeight / 2);
        }

        // EXIT STATE
        public override void Exit()
        {
            displayManager.eraser.EraseAll(); 
            displayManager.printer.PrintCenteredInWindow("Loaded", displayManager.windowInfo.WindowHeight / 2);
            Thread.Sleep(500);
            displayManager.eraser.EraseAll();
        }

        // UPDATE LOGIC
        public override void UpdateDisplay() {}

        // UPDATE DISPLAY
        public override void UpdateLogic() {}
    }
}
