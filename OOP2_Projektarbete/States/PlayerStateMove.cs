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
    internal class PlayerStateMove : PlayerStateBase
    {
        public static event Action? OnPauseMenuRequested;
        public PlayerStateMove(Player player) : base(player) { }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void MoveInput(Vector2Int direction)
        {
            player.Move(direction);
        }
        public override void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    break;
                case InputCommands.Cancel:
                    OnPauseMenuRequested?.Invoke();
                    break;
                case InputCommands.Interact:
                    player.playerStateMachine.ChangeState(PlayerStates.PlayerStateLook);
                    break;
                case InputCommands.Inventory:
                    player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMenu);
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
    }
}
