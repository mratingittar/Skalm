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
    internal class PlayerStateMenu : PlayerStateBase
    {
        public PlayerStateMenu(Player player) : base(player) { }

        public override void Enter()
        {

        }

        public override void Exit()
        {

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
                    break;
                case InputCommands.Cancel:
                    player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
                    break;
                case InputCommands.Interact:
                    break;
                case InputCommands.Inventory:
                    break;
                case InputCommands.Attack:
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
