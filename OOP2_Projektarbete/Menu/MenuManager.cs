using Skalm.Display;
using Skalm.Input;
using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.Menu
{
    internal class MenuManager
    {
        private InputManager inputManager;
        private DisplayManager displayManager;
        private AsciiArt ascii;

        private Menu activeMenu;
        private Menu mainMenu;
        //private Menu pauseMenu;

        public MenuManager(InputManager inputManager, DisplayManager displayManager)
        {
            this.inputManager = inputManager;
            this.displayManager = displayManager;
            inputManager.onInputMove += TraverseMenu;
            inputManager.onInputCommand += ExecuteMenu;
            ascii = new AsciiArt();

            mainMenu = new Menu(ascii.Title, new TreeNode<MenuPage>(new MenuPage("Main Menu", "New Game", "Continue", "Options", "Exit")), displayManager);
            //pauseMenu = new Menu();
            activeMenu = mainMenu;
        }

        public void LoadMainMenu()
        {
            activeMenu = mainMenu;
            mainMenu.LoadMenu(3);
        }

        private void TraverseMenu(Vector2Int direction)
        {
            if (activeMenu.IsEnabled is false) 
                return;

            switch (direction.Y)
            {
                case < 0:
                    activeMenu.MoveMenuDown();
                    break;
                case > 0:
                    activeMenu.MoveMenuUp();
                    break;
            }
        }

        private void ExecuteMenu(InputCommands command)
        {
            if (activeMenu.IsEnabled is false)
                return;

            SoundManager.PlayConfirmBeep();

            switch (command)
            {
                case InputCommands.Confirm:
                        activeMenu.ExecuteSelectedMenuItem();
                    break;
                case InputCommands.Cancel:
                    if (activeMenu.MenuLevel == 0)
                        ExitMenu();
                    else
                        activeMenu.GoBackOneLevel();
                    break;
            }
        }

        private void ExitMenu()
        {
            if (activeMenu.pages.Value.pageName.ToUpper() == "MAIN MENU")
                Environment.Exit(0);
            else
                LoadMainMenu();
        }
    }
}
