using Skalm.Display;
using Skalm.GameObjects;
using Skalm.Input;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class PlayerStateMessage : PlayerStateBase
    {
        private DisplayManager _displayManager;
        public PlayerStateMessage(Player player, DisplayManager displayManager) : base(player) 
        { 
            _displayManager = displayManager;
        }


        public override void Enter()
        {
            ReadMessage();
        }

        public override void Exit()
        {
            _displayManager.ClearMessageSection();
            Console.CursorVisible = false;
        }

        public override void MoveInput(Vector2Int direction)
        {
            
        }
        public override void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    ReadMessage();
                    break;
                case InputCommands.Cancel:
                    ReadMessage();
                    break;
                case InputCommands.Interact:
                    ReadMessage();
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

        private void ReadMessage()
        {
            if (_displayManager.MessagesInQueue == 0)
                player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
            else
            _displayManager.DisplayNextMessage();
        }
    }
}
