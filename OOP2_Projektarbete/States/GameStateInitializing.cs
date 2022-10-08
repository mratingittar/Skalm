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
        // ENTER STATE
        public void Enter()
        {
            DisplayManager.PrintCenteredText("Loading SKÄLM", Console.WindowHeight / 2);
        }

        // EXIT STATE
        public void Exit()
        {
            Console.Clear();
            DisplayManager.PrintCenteredText("Loaded", Console.WindowHeight / 2);
            Thread.Sleep(1000);
        }

        // UPDATE LOGIC
        public void UpdateDisplay() {}

        // UPDATE DISPLAY
        public void UpdateLogic() {}
    }
}
