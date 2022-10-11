using Skalm.Display;
using Skalm.Input;
using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.Menu
{
    internal class MenuManager
    {
        private readonly InputManager inputManager;
        private readonly DisplayManager displayManager;
        private readonly SoundManager soundManager;
        private readonly ISoundPlayer soundPlayer;
        private readonly AsciiArt ascii;

        public readonly Menu mainMenu;
        public readonly Menu pauseMenu;
        private Menu activeMenu;

        public MenuManager(InputManager inputManager, DisplayManager displayManager, SoundManager soundManager)
        {
            this.inputManager = inputManager;
            this.displayManager = displayManager;
            this.soundManager = soundManager;
            soundPlayer = soundManager.player;

            ascii = new AsciiArt();

            Dictionary<string, MenuPage> menuPagesToLoad = CreateMenuPages();
            mainMenu = new Menu(ascii.Title, new TreeNode<MenuPage>(menuPagesToLoad["MAIN MENU"], menuPagesToLoad["NEW GAME"], menuPagesToLoad["OPTIONS"], menuPagesToLoad["CREDITS"]), displayManager);
            mainMenu.pages.FindNode(node => node.Value.pageName == "OPTIONS").AddChildren(menuPagesToLoad["INPUT METHOD"], menuPagesToLoad["MUSIC"]);
            pauseMenu = new Menu(ascii.Title, new TreeNode<MenuPage>(menuPagesToLoad["PAUSE MENU"], menuPagesToLoad["OPTIONS"]), displayManager);
            activeMenu = mainMenu;
            
        }

        private Dictionary<string, MenuPage> CreateMenuPages()
        {
            List<string> inputs = inputManager.Inputs.Select(input => input.GetType().Name).ToList();
            inputs.Add("Back");

            List<string> music = soundManager.Tracks.Select(sound => sound.soundName).ToList();
            music.Add("Back");

            return new Dictionary<string, MenuPage>
            {
                {"MAIN MENU", new MenuPage("MAIN MENU", "New Game", "Options", "Credits", "Exit")},
                {"NEW GAME", new MenuPage("NEW GAME", "Start New Game", "Back")},
                {"OPTIONS", new MenuPage("OPTIONS", "Input Method", "Music", "Toggle Beep", "Back")},
                {"CREDITS", new MenuPage("CREDITS", "Josef Schönbäck", "Martin Lindvik", "Music by Kevin MacLeod(incompetech.com)", "Licensed under Creative Commons: By Attribution 4.0 License", "Back")},
                {"INPUT METHOD", new MenuPage("INPUT METHOD", inputs.ToArray())},
                {"MUSIC", new MenuPage("MUSIC", music.ToArray())},
                {"PAUSE MENU", new MenuPage("PAUSE MENU", "Resume", "Options", "Exit")}
            };
        }


        public void LoadMenu(Menu menu)
        {
            activeMenu = menu;
            menu.LoadMenu(3);
            activeMenu.IsEnabled = true;
        }

        public void UnloadMenu()
        {
            activeMenu.IsEnabled = false;
            displayManager.eraser.EraseAll();
        }

        public void TraverseMenu(Vector2Int direction)
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

        public void ExecuteMenu(InputCommands command)
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
                    activeMenu.Cancel();
                    break;
            }
        }
    }
}
