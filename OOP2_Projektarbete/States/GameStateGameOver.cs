using Skalm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStateGameOver : GameStateBase
    {
        public GameStateGameOver(GameManager gameManager) : base(gameManager)
        {
        }

        public override void Enter()
        {
            inputManager.OnInputCommand += CommandInput;
            displayManager.Printer.PrintCenteredInWindow("YOU DIED.", displayManager.WindowInfo.WindowHeight/2);
        }

        public override void Exit()
        {
            sceneManager.ResetScene();
            inputManager.OnInputCommand -= CommandInput;
        }

        private void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    gameManager.stateMachine.ChangeState(GameStates.GameStateMainMenu);
                    break;
                case InputCommands.Cancel:
                    break;
                case InputCommands.Interact:
                    break;
                case InputCommands.Inventory:
                    break;
                case InputCommands.Next:
                    break;
                case InputCommands.Previous:
                    break;
                case InputCommands.Help:
                    break;
                default:
                    break;
            }
        }

        public override void UpdateDisplay()
        {
            
        }

        public override void UpdateLogic()
        {
            
        }
    }
}
