using Skalm.Display;
using Skalm.Input;
using Skalm.Menu;
using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.States
{
    internal class GameStatePaused : IGameState
    {
        private GameManager _gameManager;
        private MenuManager _menuManager;
        private SoundManager _soundManager;
        private InputManager _inputManager;
        private SceneManager _sceneManager;

        // CONSTRUCTOR I
        public GameStatePaused(GameManager gameManager)
        {
            _gameManager = gameManager;
            _menuManager = gameManager.MenuManager;
            _soundManager = gameManager.SoundManager;
            _inputManager = gameManager.InputManager;
            _sceneManager = gameManager.SceneManager;
        }

        #region StateMachine Basics
        // STATE ENTER
        public void Enter()
        {
            _menuManager.LoadMenu(_menuManager.pauseMenu);

            // INPUT EVENT SUBSCRIBE
            _inputManager.OnInputMove += MoveInput;
            _inputManager.OnInputCommand += CommandInput;

            // MENU EVENT SUBSCRIBE
            _menuManager.pauseMenu.OnMenuExecution += MenuExecution;
        }

        // STATE EXIT
        public void Exit()
        {
            _menuManager.UnloadMenu();

            // INPUT EVENT UNSUBSCRIBE
            _inputManager.OnInputMove -= MoveInput;
            _inputManager.OnInputCommand -= CommandInput;

            // MENU EVENT UNSUBSCRIBE
            _menuManager.pauseMenu.OnMenuExecution -= MenuExecution;
        }

        // STATE UPDATE LOGIC
        public void UpdateLogic()
        {

        }

        // STATE UPDATE DISPLAY
        public void UpdateDisplay()
        {

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

        private void MenuExecution(Page menuPage, string item)
        {
            switch (menuPage)
            {
                case Page.PauseMenu:
                    if (item == "Resume")
                        _gameManager.StateMachine.ChangeState(GameStates.GameStatePlaying);
                    else if (item == "Exit")
                    {
                        _sceneManager.ResetScene();
                        _gameManager.StateMachine.ChangeState(GameStates.GameStateMainMenu);
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

        #endregion
    }
}
