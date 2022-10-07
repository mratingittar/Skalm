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
        private MenuManager menuManager;

        public GameStateMainMenu(MenuManager menuManager)
        {
            this.menuManager = menuManager;
        }

        public void Enter()
        {
            Console.Clear();
            menuManager.LoadMainMenu();
        }

        public void Exit()
        {
            //MainMenu.Enabled = false;
            Console.Clear();
        }
    }
}
