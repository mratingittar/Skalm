using Skalm.Animation;
using Skalm.Input;
using Skalm.Menu;
using Skalm.Structs;

namespace Skalm.States
{
    internal class GameStateMainMenu : GameStateBase
    {
        public string playerName = "";
        private Animator _fireAnimator;
        private bool _everyOtherFrame = true;
        private const int _menuPageHeight = 6;

        // CONSTRUCTOR I
        public GameStateMainMenu(GameManager gameManager) : base(gameManager)
        {
            _fireAnimator = gameManager.FireAnimator;
        }

        #region StateBasics

        // ENTER STATE
        public override void Enter()
        {
            _displayManager.Eraser.EraseAll();
            _menuManager.LoadMenu(_menuManager.mainMenu);
            _soundManager.PlayMusic(_soundManager.Tracks.Find(song => song.soundName == "Video Dungeon Crawl"));
            _sceneManager.playerName = "";

            // INPUT EVENT SUBSCRIBE
            _inputManager.OnInputMove += MoveInput;
            _inputManager.OnInputCommand += CommandInput;

            // MENU EVENT SUBSCRIBE
            _menuManager.mainMenu.onMenuExecution += MenuExecution;
        }

        // EXIT STATE
        public override void Exit()
        {
            _menuManager.UnloadMenu();

            // INPUT EVENT UNSUBSCRIBE
            _inputManager.OnInputMove -= MoveInput;
            _inputManager.OnInputCommand -= CommandInput;

            // MENU EVENT UNSUBSCRIBE
            _menuManager.mainMenu.onMenuExecution -= MenuExecution;
        }

        // UPDATE LOGIC
        public override void UpdateLogic()
        {

        }

        // UPDATE DISPLAY
        public override void UpdateDisplay()
        {
            if (_everyOtherFrame)
            {
                _fireAnimator.AnimatedBraziers();
                _everyOtherFrame = false;
            }
            else
                _everyOtherFrame = true;
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
                            _gameManager.SceneManager.playerName = nameReturned;
                        else
                            EraseRow(Console.CursorTop);
                    }

                    if (item == "Start New Game")
                    {
                        _gameManager.NewGame = true;
                        _gameManager.stateMachine.ChangeState(GameStates.GameStatePlaying);
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
                    name = name.Substring(0, name.Length - 1);
                    EraseRow(height);
                }

                else
                    name += cki.KeyChar;

                _displayManager.Printer.PrintCenteredInWindow(name, height);
            }

            if (name.Length == 0)
                return (false, name);
            else
                return (true, name);
        }

        private void EraseRow(int row)
        {
            _displayManager.Eraser.EraseLinesFromTo(row, row);
        }
        #endregion
    }
}
