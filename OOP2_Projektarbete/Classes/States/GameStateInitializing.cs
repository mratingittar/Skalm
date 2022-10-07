using OOP2_Projektarbete.Classes.Managers;
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
            DisplayManager.PrintCenteredText("Loading SKÄLM", Console.WindowHeight / 2);            
        }

        public void Exit()
        {
            Console.Clear();
            DisplayManager.PrintCenteredText("Loaded", Console.WindowHeight / 2);
            Thread.Sleep(1000);
        }
    }
}
