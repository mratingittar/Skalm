using Skalm.Display;
using Skalm.Menu;

namespace Skalm.States
{
    internal class GameStateMainMenu : IGameState
    {
        private MenuManager menuManager;
        private DisplayManager displayManager;


        public GameStateMainMenu(DisplayManager displayManager, MenuManager menuManager)
=======
        // CONSTRUCTOR I

        {
            this.displayManager = displayManager;
            this.menuManager = menuManager;
        }

        // ENTER STATE
        public void Enter()
        {
            displayManager.eraser.EraseAll();
            menuManager.LoadMainMenu();
        }

        // EXIT STATE
        public void Exit()
        {
            menuManager.mainMenu.IsEnabled = false;
            displayManager.eraser.EraseAll();
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
