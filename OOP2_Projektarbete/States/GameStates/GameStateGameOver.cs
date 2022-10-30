using Skalm.Animation;
using Skalm.Display;
using Skalm.Input;
using Skalm.Maps;
using Skalm.Sounds;

namespace Skalm.States.GameStates
{
    internal class GameStateGameOver : IGameState
    {
        private GameManager _gameManager;
        private DisplayManager _displayManager;
        private SoundManager _soundManager;
        private InputManager _inputManager;
        private SceneManager _sceneManager;
        private MapManager _mapManager;
        private Animator _animator;

        // CONSTRUCTOR I
        public GameStateGameOver(GameManager gameManager)
        {
            _gameManager = gameManager;
            _displayManager = gameManager.DisplayManager;
            _soundManager = gameManager.SoundManager;
            _inputManager = gameManager.InputManager;
            _sceneManager = gameManager.SceneManager;
            _mapManager = gameManager.MapManager;
            _animator = gameManager.Animator;
        }

        // ENTER STATE
        public void Enter()
        {
            _soundManager.PlayMusic(_soundManager.Tracks.Find(song => song.soundName == "Steel and Seething"));
            _inputManager.OnInputCommand += CommandInput;
            _displayManager.Printer.PrintCenteredInWindow("YOU DIED.", _displayManager.WindowInfo.WindowHeight / 2 + 8, _gameManager.Settings.TextColor);
            _sceneManager.ResetScene();
            _mapManager.MapGenerator.ResetMapGenerator();
        }

        // EXIT STATE
        public void Exit()
        {
            _inputManager.OnInputCommand -= CommandInput;
            _displayManager.ClearMessageQueue();
        }

        // COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    _gameManager.StateMachine.ChangeState(EGameStates.GameStateMainMenu);
                    break;
                case InputCommands.Cancel:
                    _gameManager.StateMachine.ChangeState(EGameStates.GameStateMainMenu);
                    break;
                case InputCommands.Interact:
                    break;
                case InputCommands.Inventory:
                    break;
                default:
                    break;
            }
        }

        // UPDATE DISPLAY
        public void UpdateDisplay()
        {
            _animator.LaughingSkull();
        }

        // UPDATE LOGIC
        public void UpdateLogic()
        {

        }
    }
}
