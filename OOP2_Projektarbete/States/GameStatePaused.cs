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

        // CONSTRUCTOR I
        public GameStatePaused(MenuManager menuManager)
        {
            this.menuManager = menuManager;
        }

        // STATE ENTER
        public void Enter()
        {
            menuManager.LoadMenu(menuManager.pauseMenu);
        }

        // STATE EXIT
        public void Exit()
        {
            menuManager.UnloadMenu();
        }

        // STATE UPDATE LOGIC
        public void UpdateLogic()
        {
            throw new NotImplementedException();
        }

        // STATE UPDATE DISPLAY
        public void UpdateDisplay()
        {
            throw new NotImplementedException();
        }
    }
}
