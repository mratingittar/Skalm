using Skalm.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStatePaused : IGameState
    {
        private MenuManager menuManager;
        public GameStatePaused(MenuManager menuManager)
        {
            this.menuManager = menuManager;
        }

        public void Enter()
        {
            menuManager.LoadMenu(menuManager.pauseMenu);
        }

        public void Exit()
        {
            menuManager.UnloadMenu();
        }

        public void UpdateDisplay()
        {
            throw new NotImplementedException();
        }

        public void UpdateLogic()
        {
            throw new NotImplementedException();
        }
    }
}
