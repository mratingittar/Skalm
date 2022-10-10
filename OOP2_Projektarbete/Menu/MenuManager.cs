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
        private ISoundPlayer soundPlayer;
        private AsciiArt ascii;

        public readonly Menu mainMenu;
        private Menu pauseMenu;
        private Menu activeMenu;

        public MenuManager(InputManager inputManager, DisplayManager displayManager, ISoundPlayer soundManager)
        {
            this.inputManager = inputManager;
            this.displayManager = displayManager;
            this.soundPlayer = soundManager;
            inputManager.onInputMove += TraverseMenu;
            inputManager.onInputCommand += ExecuteMenu;

            ascii = new AsciiArt();

            Dictionary<string, MenuPage> menuPagesToLoad = CreateMenuPages();
            mainMenu = new Menu(ascii.Title, new TreeNode<MenuPage>(menuPagesToLoad["MAIN MENU"], menuPagesToLoad["NEW GAME"], menuPagesToLoad["OPTIONS"]), displayManager);
            mainMenu.pages.FindNode(node => node.Value.pageName == "OPTIONS").AddChildren(menuPagesToLoad["INPUT METHOD"], menuPagesToLoad["MUSIC"]);
            pauseMenu = new Menu(ascii.Title, new TreeNode<MenuPage>(menuPagesToLoad["PAUSE MENU"]), displayManager);
            activeMenu = mainMenu;
        }

        private Dictionary<string, MenuPage> CreateMenuPages()
        {
            return new Dictionary<string, MenuPage>
            {
                {"MAIN MENU", new MenuPage("MAIN MENU", "New Game", "Continue", "Options", "Exit")},
                {"NEW GAME",new MenuPage("NEW GAME", "Enter Name", "Start New Game", "Back")},
                {"OPTIONS",new MenuPage("OPTIONS", "Input Method", "Music", "Toggle Beep", "Back")},
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
                    if (activeMenu.MoveMenuDown())
                        soundPlayer.Play(SoundManager.SoundType.Move);
                    break;
                case > 0:
                    if (activeMenu.MoveMenuUp())
                        soundPlayer.Play(SoundManager.SoundType.Move);
                    break;
            }
        }

        private void ExecuteMenu(InputCommands command)
        {
            if (activeMenu.IsEnabled is false)
                return;

            soundPlayer.Play(SoundManager.SoundType.Confirm);

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
