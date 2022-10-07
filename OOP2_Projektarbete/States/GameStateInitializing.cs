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
            DisplayManager.Print("Loading SKÄLM", 0, Console.WindowHeight / 2, true);
        }

        public void Exit()
        {
            Console.Clear();
            DisplayManager.Print("Loaded", 0, Console.WindowHeight / 2, true);
            Thread.Sleep(1000);
        }
    }
}
