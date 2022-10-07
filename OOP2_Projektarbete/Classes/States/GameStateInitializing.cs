using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.States
{
    internal class GameStateInitializing : IGameState
    {
        public void Enter()
        {
            Console.WriteLine("Initializing...");
        }

        public void Exit()
        {
            Console.WriteLine("Initialized.");
        }
    }
}
