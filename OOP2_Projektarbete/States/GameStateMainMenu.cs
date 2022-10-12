using Skalm.Display;
using Skalm.Menu;

namespace Skalm.States
{
    internal class GameStateMainMenu : IGameState
    {
        private MenuManager menuManager;

        // CONSTRUCTOR I
        public GameStateMainMenu(MenuManager menuManager)
        {
            this.menuManager = menuManager;
        }

        // ENTER STATE
        public void Enter()
        {
            menuManager.LoadMenu(menuManager.mainMenu);
        }

        // EXIT STATE
        public void Exit()
        {
            menuManager.UnloadMenu();
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
