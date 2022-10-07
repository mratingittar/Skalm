using OOP2_Projektarbete.Classes.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.States
{
    internal class GameStatePlaying : IGameState
    {
        private readonly DisplayManager _displayManager;

        public GameStatePlaying(DisplayManager displayManager)
        {
            _displayManager = displayManager;
        }

        public void Enter()
        {
            DisplayManager.PrintCenteredText("Starting new game", Console.WindowHeight/2);            
            Thread.Sleep(500);
            Console.Clear();
            _displayManager.DisplayHUD();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }
    
    }
}
