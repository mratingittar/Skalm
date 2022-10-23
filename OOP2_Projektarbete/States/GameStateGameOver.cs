using Skalm.Display;
using Skalm.Input;
using Skalm.Menu;
using Skalm.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStateGameOver : IGameState
    {
        private GameManager _gameManager;
        private DisplayManager _displayManager;
        private InputManager _inputManager;
        private SceneManager _sceneManager;

        public GameStateGameOver(GameManager gameManager)
        {
            _gameManager = gameManager;
            _displayManager = gameManager.DisplayManager;
            _inputManager = gameManager.InputManager;
            _sceneManager = gameManager.SceneManager;
        }

        public void Enter()
        {
            _inputManager.OnInputCommand += CommandInput;
            _displayManager.Printer.PrintCenteredInWindow("YOU DIED.", _displayManager.WindowInfo.WindowHeight/2);
        }

        public void Exit()
        {
            _sceneManager.ResetScene();
            _inputManager.OnInputCommand -= CommandInput;
        }

        private void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    _gameManager.stateMachine.ChangeState(GameStates.GameStateMainMenu);
                    break;
                case InputCommands.Cancel:
                    break;
                case InputCommands.Interact:
                    break;
                case InputCommands.Inventory:
                    break;
                default:
                    break;
            }
        }

        public void UpdateDisplay()
        {
            
        }

        public void UpdateLogic()
        {
            
        }
    }
}
