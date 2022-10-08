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
        public void Enter()
        {
            DisplayManager.PrintCentered("Loading SKÄLM", Console.WindowHeight / 2);
        }

        public void Exit()
        {
            Console.Clear();
            DisplayManager.PrintCentered("Loaded", Console.WindowHeight / 2);
            Thread.Sleep(1000);
        }
    }
}
