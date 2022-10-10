using Skalm.Display;
using Skalm.Menu;

namespace Skalm.States
{
    internal class GameStateMainMenu : IGameState
    {
        private MenuManager menuManager;
        private DisplayManager displayManager;

        public GameStateMainMenu(DisplayManager displayManager, MenuManager menuManager)
        {
            this.displayManager = displayManager;
            this.menuManager = menuManager;
        }

        public void Enter()
        {
            displayManager.eraser.EraseAll();
            menuManager.LoadMainMenu();
        }

        public void Exit()
        {
            menuManager.mainMenu.IsEnabled = false;
            displayManager.eraser.EraseAll();
        }
    }
}
