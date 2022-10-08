using Skalm.Display;
using Skalm.Input;
using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.Menu
{
    internal class MenuManager
    {
        private InputManager inputManager;
        private AsciiArt ascii;

        private MenuBase activeMenu;
        private MainMenu mainMenu;
        //private PauseMenu pauseMenu;

        public MenuManager(InputManager inputManager)
        {
            this.inputManager = inputManager;

            inputManager.onInputMove += TraverseMenu;
            inputManager.onInputCommand += ExecuteMenu;
            ascii = new AsciiArt();

            mainMenu = new MainMenu(ascii.Title, new TreeNode<MenuPage>(new MenuPage("Main Menu", "New Game", "Continue", "Options", "Exit")));
            //pauseMenu = new PauseMenu();
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
            if (activeMenu is MainMenu)
                Environment.Exit(0);
            else
                LoadMainMenu();
        }

    }
}
