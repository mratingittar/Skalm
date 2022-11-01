using Skalm.Animation;
using Skalm.Display;
using Skalm.Input;
using Skalm.Maps;
using Skalm.Menu;
using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.States.GameStates
{
    internal class GameStateMainMenu : IGameState
    {
        public string playerName = "";

        private const int _menuPageHeight = 7;
        private Animator _fireAnimator;
        private GameManager _gameManager;
        private DisplayManager _displayManager;
        private MenuManager _menuManager;
        private SoundManager _soundManager;
        private InputManager _inputManager;
        private SceneManager _sceneManager;
        private MapManager _mapManager;

        // CONSTRUCTOR I
        public GameStateMainMenu(GameManager gameManager)
        {
            _gameManager = gameManager;
            _displayManager = gameManager.DisplayManager;
            _menuManager = gameManager.MenuManager;
            _soundManager = gameManager.SoundManager;
            _inputManager = gameManager.InputManager;
            _fireAnimator = gameManager.Animator;
            _sceneManager = gameManager.SceneManager;
            _mapManager = gameManager.MapManager;
        }

        #region StateBasics

        // ENTER STATE
        public void Enter()
        {
            _displayManager.Eraser.EraseAll();
            _menuManager.LoadMenu(_menuManager.mainMenu);
            _soundManager.PlayMusic(_soundManager.Tracks.Find(song => song.soundName == "Video Dungeon Crawl"));
            _sceneManager.PlayerName = "";

            // INPUT EVENT SUBSCRIBE
            _inputManager.OnInputMove += MoveInput;
            _inputManager.OnInputCommand += CommandInput;

            // MENU EVENT SUBSCRIBE
            _menuManager.mainMenu.OnMenuExecution += MenuExecution;
        }

        // EXIT STATE
        public void Exit()
        {
            _menuManager.UnloadMenu();

            // INPUT EVENT UNSUBSCRIBE
            _inputManager.OnInputMove -= MoveInput;
            _inputManager.OnInputCommand -= CommandInput;

            // MENU EVENT UNSUBSCRIBE
            _menuManager.mainMenu.OnMenuExecution -= MenuExecution;
        }

        // UPDATE LOGIC
        public void UpdateLogic()
        {

        }

        // UPDATE DISPLAY
        public void UpdateDisplay()
        {
            _fireAnimator.AnimatedBraziers();
        }

        #endregion

        #region Methods

        // METHOD MOVE INPUT
        private void MoveInput(Vector2Int direction)
        {
            _menuManager.TraverseMenu(direction);
        }

        // METHOD COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            _menuManager.ExecuteMenu(command);
        }

        // METHOD EXECUTE MENU INDEX
        private void MenuExecution(Page menuPage, string item)
        {
            switch (menuPage)
            {
                case Page.MainMenu:
                    if (item == "Exit")
                        Environment.Exit(0);
                    break;

                case Page.NewGame:
                    if (item == "Enter Name")
                    {
                        EraseRow(_menuManager.ActiveMenu.PageStartRow + _menuPageHeight);
                        (bool nameOK, string nameReturned) = EnterName(_menuManager.ActiveMenu.PageStartRow + _menuPageHeight);
                        Console.CursorVisible = false;
                        if (nameOK)
                            _gameManager.SceneManager.PlayerName = nameReturned.Trim();
                        else
                            EraseRow(Console.CursorTop);
                    }

                    if (item == "Start New Game")
                    {
                        _mapManager.MapGenerator.UseRandomMaps = false;
                        _gameManager.NewGame = true;
                        _gameManager.StateMachine.ChangeState(EGameStates.GameStatePlaying);
                    }

                    if (item == "Start Random Game")
                    {
                        _mapManager.MapGenerator.UseRandomMaps = true;
                        _gameManager.NewGame = true;
                        _gameManager.StateMachine.ChangeState(EGameStates.GameStatePlaying);
                    }
                    break;

                case Page.Options:
                    if (item == "Toggle Beep")
                        _soundManager.player.SFXEnabled = !_soundManager.player.SFXEnabled;
                    break;

                case Page.Music:
                    _soundManager.PlayMusic(_soundManager.Tracks.Find(sound => sound.soundName == item));
                    _menuManager.ActiveMenu.ReloadPage();
                    break;

                case Page.InputMethod:
                    _inputManager.SetInputMethod(_inputManager.Inputs.Find(input => input.GetType().Name == item)!);
                    _menuManager.ActiveMenu.ReloadPage();
                    break;
            }
        }

        // METHOD NAME ENTERING
        private (bool, string) EnterName(int height)
        {
            Console.SetCursorPosition(_displayManager.WindowInfo.WindowWidth / 2, height);
            Console.CursorVisible = true;
            ConsoleKeyInfo cki;
            string name = "";
            while (true)
            {
                cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.Enter)
                    break;

                else if (cki.Key == ConsoleKey.Escape)
                    return (false, name);

                else if (cki.Key == ConsoleKey.Backspace && name.Length > 0)
                {
                    name = Backspace(name);
                    EraseRow(height);
                }

                else
                    name += cki.KeyChar;

                _displayManager.Printer.PrintCenteredInWindow(name, height, _gameManager.Settings.TextColor);
            }

            if (name.Length == 0)
                return (false, name);
            else
                return (true, name);
        }

        private static string Backspace(string name)
        {
            name = name.Substring(0, name.Length - 1);
            return name;
        }

        private void EraseRow(int row)
        {
            _displayManager.Eraser.EraseLinesFromTo(row, row);
        }
        #endregion
    }
}
