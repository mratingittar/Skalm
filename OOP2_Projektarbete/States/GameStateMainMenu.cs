using Skalm.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStateMainMenu : IGameState
    {
        public MainMenu MainMenu { get; private set; }

        // CONSTRUCTOR I
        public GameStateMainMenu(MainMenu menu)
        {
            MainMenu = menu;
        }

        // ENTER STATE
        public void Enter()
        {
            Console.Clear();
            MainMenu.LoadMenu();
        }

        // EXIT STATE
        public void Exit()
        {
            MainMenu.Enabled = false;
            Console.Clear();
        }

        // UPDATE LOGIC
        public void UpdateLogic()
        {
            throw new NotImplementedException();
        }

        // UPDATE DISPLAY
        public void UpdateDisplay()
        {
            throw new NotImplementedException();
        }
    }
}
