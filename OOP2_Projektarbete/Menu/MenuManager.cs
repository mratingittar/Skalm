using Skalm.Display;
using Skalm.Input;
using Skalm.Sounds;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Menu
{
    internal class MenuManager
    {
        public Menu ActiveMenu { get; private set; }
        public readonly Menu mainMenu;
        public readonly Menu pauseMenu;

        private readonly InputManager _inputManager;
        private readonly DisplayManager _displayManager;
        private readonly SoundManager _soundManager;
        private readonly ISoundPlayer _soundPlayer;

        public MenuManager(InputManager inputManager, DisplayManager displayManager, SoundManager soundManager, ISettings settings)
        {
            _inputManager = inputManager;
            _displayManager = displayManager;
            _soundManager = soundManager;
            _soundPlayer = soundManager.player;

            if (!FileHandler.TryReadFile("skalm_title.txt", out string[] title))
                title = new string[0];

            Dictionary<string, MenuPage> menuPagesToLoad = CreateMenuPages();
            mainMenu = new Menu(title, new TreeNode<MenuPage>(menuPagesToLoad["MAIN MENU"], menuPagesToLoad["NEW GAME"], menuPagesToLoad["OPTIONS"], menuPagesToLoad["HOW TO PLAY"], menuPagesToLoad["CREDITS"]), displayManager, soundManager, inputManager, settings.TextColor);
            mainMenu.pages.FindNode(node => node.Value.pageName == "OPTIONS").AddChildren(menuPagesToLoad["INPUT METHOD"], menuPagesToLoad["MUSIC"]);
            pauseMenu = new Menu(title, new TreeNode<MenuPage>(menuPagesToLoad["PAUSE MENU"], menuPagesToLoad["OPTIONS"], menuPagesToLoad["HOW TO PLAY"]), displayManager, soundManager, inputManager, settings.TextColor);
            pauseMenu.pages.FindNode(node => node.Value.pageName == "OPTIONS").AddChildren(menuPagesToLoad["INPUT METHOD"], menuPagesToLoad["MUSIC"]);
            ActiveMenu = mainMenu;
        }

        private Dictionary<string, MenuPage> CreateMenuPages()
        {
            List<string> inputs = _inputManager.Inputs.Select(input => input.GetType().Name).ToList();
            inputs.Add("Back");

            List<string> music = _soundManager.Tracks.Select(sound => sound.soundName).ToList();
            music.Add("Back");

            string[] helpText = new string[]
            {
                "You are a SKÄLM.",
                "A rogue, seeking to loot riches and slay monsters,",
                "as you venture ever deeper into the dungeon.",
                "CONTROLS:",
                "Interact........E",
                "Inventory.......I",
                "Confirm.....Enter",
                "Cancel.....Escape",
                "Back"
            };

            return new Dictionary<string, MenuPage>
            {
                {"MAIN MENU", new MenuPage(Page.MainMenu, "MAIN MENU", "New Game", "Options", "How To Play", "Credits", "Exit")},
                {"NEW GAME", new MenuPage(Page.NewGame, "NEW GAME", "Start New Game", "Enter Name", "Back")},
                {"OPTIONS", new MenuPage(Page.Options, "OPTIONS", "Input Method", "Music", "Toggle Beep", "Back")},
                {"CREDITS", new MenuPage(Page.Credits, "CREDITS", "Josef Schönbäck", "Martin Lindvik", "Music by Kevin MacLeod(incompetech.com)", "Licensed under Creative Commons: By Attribution 4.0 License", "Back")},
                {"INPUT METHOD", new MenuPage(Page.InputMethod, "INPUT METHOD", inputs.ToArray())},
                {"MUSIC", new MenuPage(Page.Music, "MUSIC", music.ToArray())},
                {"HOW TO PLAY", new MenuPage(Page.HowToPlay, "HOW TO PLAY", helpText)},
                {"PAUSE MENU", new MenuPage(Page.PauseMenu, "PAUSE MENU", "Resume", "Options", "How To Play", "Exit")}
            };
        }

        public void LoadMenu(Menu menu)
        {
            ActiveMenu = menu;
            menu.LoadMenu(3);
        }

        public void UnloadMenu()
        {
            ActiveMenu.IsEnabled = false;
            _displayManager.Eraser.EraseAll();
        }

        public void TraverseMenu(Vector2Int direction)
        {
            if (ActiveMenu.IsEnabled is false)
                return;

            switch (direction.Y)
            {
                case < 0:
                    if (ActiveMenu.MoveMenuUp())
                        _soundPlayer.Play(SoundManager.SoundType.Low);
                    break;
                case > 0:
                    if (ActiveMenu.MoveMenuDown())
                        _soundPlayer.Play(SoundManager.SoundType.Low);
                    break;
            }
        }

        public void ExecuteMenu(InputCommands command)
        {
            if (ActiveMenu.IsEnabled is false)
                return;

            _soundPlayer.Play(SoundManager.SoundType.Mid);

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
