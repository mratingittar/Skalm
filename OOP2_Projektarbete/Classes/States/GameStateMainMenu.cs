using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.States
{
    internal class GameStateMainMenu : IGameState
    {
        public MainMenu MainMenu { get; private set; }

        public GameStateMainMenu(MainMenu menu)
        {
            MainMenu = menu;
        }

        public void Enter()
        {
            Console.Clear();
            MainMenu.LoadMenu();
        }

        public void Exit()
        {
            MainMenu.Enabled = false;
            Console.Clear();
        }
    }
}
