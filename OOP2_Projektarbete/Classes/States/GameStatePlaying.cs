using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.States
{
    internal class GameStatePlaying : IGameState
    {
        public void Enter()
        {
            Console.WriteLine("Starting New Game");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("Game is running...");
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }
    
    }
}
