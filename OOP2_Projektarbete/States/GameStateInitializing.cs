using Skalm.Display;
using Skalm.Input;
using Skalm.Menu;
using Skalm.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStateInitializing : IGameState
    {
        private DisplayManager _displayManager;

        // CONSTRUCTOR I
        public GameStateInitializing(GameManager gameManager)
        {
            _displayManager = gameManager.DisplayManager;
        }

        // ENTER STATE
        public void Enter()
        {
            _displayManager.Printer.PrintCenteredInWindow("Loading SKÄLM", _displayManager.WindowInfo.WindowHeight / 2);
        }

        // EXIT STATE
        public void Exit()
        {
            _displayManager.Eraser.EraseAll(); 
            _displayManager.Printer.PrintCenteredInWindow("Loaded", _displayManager.WindowInfo.WindowHeight / 2);
            Thread.Sleep(500);
            _displayManager.Eraser.EraseAll();
        }

        // UPDATE LOGIC
        public void UpdateDisplay() {}

        // UPDATE DISPLAY
        public void UpdateLogic() {}
    }
}
