using Skalm.Display;
using Skalm.Input;
using Skalm.Sounds;
using Skalm.Structs;
using System.Text.Json;

namespace Skalm.Menu
{
    internal class MenuManager
    {
        private InputManager inputManager;
        private DisplayManager displayManager;
        private AsciiArt ascii;

        public readonly Menu mainMenu;
        private Menu pauseMenu;
        private Menu activeMenu;
        private Dictionary<string,MenuPage> menuPagesToLoad;

        public MenuManager(InputManager inputManager, DisplayManager displayManager)
        {
            this.inputManager = inputManager;
            this.displayManager = displayManager;
            inputManager.onInputMove += TraverseMenu;
            inputManager.onInputCommand += ExecuteMenu;

            ascii = new AsciiArt();

            menuPagesToLoad = CreateMenuPages();
            mainMenu = new Menu(ascii.Title, new TreeNode<MenuPage>(menuPagesToLoad["MAIN MENU"], menuPagesToLoad["NEW GAME"], menuPagesToLoad["OPTIONS"]), displayManager);
            mainMenu.pages.FindNode(node => node.Value.pageName == "OPTIONS").AddChildren(menuPagesToLoad["INPUT METHOD"], menuPagesToLoad["MUSIC"]);
            pauseMenu = new Menu(ascii.Title, new TreeNode<MenuPage>(menuPagesToLoad["PAUSE MENU"]), displayManager);
            activeMenu = mainMenu;
        }

        private Dictionary<string, MenuPage> CreateMenuPages()
        {
            return new Dictionary<string, MenuPage>
            {
                { "MAIN MENU", new MenuPage("MAIN MENU", "New Game", "Continue", "Options", "Exit") },
                { "NEW GAME",new MenuPage("NEW GAME", "Enter Name", "Start New Game", "Back")},
                { "OPTIONS",new MenuPage("OPTIONS", "Input Method", "Music", "Toggle Beep", "Back")},
                {"INPUT METHOD", new MenuPage("INPUT METHOD", "Back")},
                {"MUSIC", new MenuPage("MUSIC", "Back")},
                {"PAUSE MENU", new MenuPage("PAUSE MENU", "Options", "Exit")}
            };
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
