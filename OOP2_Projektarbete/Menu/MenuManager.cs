using Skalm.Display;
using Skalm.Input;
using Skalm.Sounds;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Menu
{
    internal class MenuManager
    {
        private readonly InputManager inputManager;
        private readonly DisplayManager displayManager;
        private readonly SoundManager soundManager;
        private readonly ISoundPlayer soundPlayer;

        public readonly Menu mainMenu;
        public readonly Menu pauseMenu;
        public Menu ActiveMenu { get; private set; }

        public MenuManager(InputManager inputManager, DisplayManager displayManager, SoundManager soundManager)
        {
            this.inputManager = inputManager;
            this.displayManager = displayManager;
            this.soundManager = soundManager;
            soundPlayer = soundManager.player;

            if (!FileHandler.TryReadFile("Skalm_Title.txt", out string[] title))
                title = new string[0];

            Dictionary<string, MenuPage> menuPagesToLoad = CreateMenuPages();
            mainMenu = new Menu(title, new TreeNode<MenuPage>(menuPagesToLoad["MAIN MENU"], menuPagesToLoad["NEW GAME"], menuPagesToLoad["OPTIONS"], menuPagesToLoad["CREDITS"]), displayManager, soundManager, inputManager);
            mainMenu.pages.FindNode(node => node.Value.pageName == "OPTIONS").AddChildren(menuPagesToLoad["INPUT METHOD"], menuPagesToLoad["MUSIC"]);
            pauseMenu = new Menu(title, new TreeNode<MenuPage>(menuPagesToLoad["PAUSE MENU"], menuPagesToLoad["OPTIONS"]), displayManager, soundManager, inputManager);
            ActiveMenu = mainMenu;
        }

        private Dictionary<string, MenuPage> CreateMenuPages()
        {
            List<string> inputs = inputManager.Inputs.Select(input => input.GetType().Name).ToList();
            inputs.Add("Back");

            List<string> music = soundManager.Tracks.Select(sound => sound.soundName).ToList();
            music.Add("Back");

            return new Dictionary<string, MenuPage>
            {
                {"MAIN MENU", new MenuPage(Page.MainMenu, "MAIN MENU", "New Game", "Options", "Credits", "Exit")},
                {"NEW GAME", new MenuPage(Page.NewGame, "NEW GAME", "Start New Game", "Back")},
                {"OPTIONS", new MenuPage(Page.Options, "OPTIONS", "Input Method", "Music", "Toggle Beep", "Back")},
                {"CREDITS", new MenuPage(Page.Credits, "CREDITS", "Josef Schönbäck", "Martin Lindvik", "Music by Kevin MacLeod(incompetech.com)", "Licensed under Creative Commons: By Attribution 4.0 License", "Back")},
                {"INPUT METHOD", new MenuPage(Page.InputMethod, "INPUT METHOD", inputs.ToArray())},
                {"MUSIC", new MenuPage(Page.Music, "MUSIC", music.ToArray())},
                {"PAUSE MENU", new MenuPage(Page.PauseMenu, "PAUSE MENU", "Resume", "Options", "Exit")}
            };
        }

        public void LoadMenu(Menu menu)
        {
            ActiveMenu = menu;
            menu.LoadMenu(3);
            ActiveMenu.IsEnabled = true;
        }

        public void UnloadMenu()
        {
            ActiveMenu.IsEnabled = false;
            displayManager.eraser.EraseAll();
        }

        public void TraverseMenu(Vector2Int direction)
        {
            if (ActiveMenu.IsEnabled is false)
                return;

            switch (direction.Y)
            {
                case < 0:
                    if (ActiveMenu.MoveMenuDown())
                        soundPlayer.Play(SoundManager.SoundType.Move);
                    break;
                case > 0:
                    if (ActiveMenu.MoveMenuUp())
                        soundPlayer.Play(SoundManager.SoundType.Move);
                    break;
            }
        }

        public void ExecuteMenu(InputCommands command)
        {
            if (ActiveMenu.IsEnabled is false)
                return;

            soundPlayer.Play(SoundManager.SoundType.Confirm);

            switch (command)
            {
                case InputCommands.Confirm:
                    ActiveMenu.ExecuteSelectedMenuItem();
                    break;
                case InputCommands.Cancel:
                    ActiveMenu.Cancel();
                    break;
            }
        }
    }
}
