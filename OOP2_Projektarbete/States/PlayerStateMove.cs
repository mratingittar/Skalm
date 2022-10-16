using Skalm.Actors;
using Skalm.Actors.Tile;
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
        public PlayerStateMove(GameManager gameManager, Player player) : base(gameManager, player) { }

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
            if (command == InputCommands.Cancel)
                gameManager.stateMachine.ChangeState(GameStates.GameStatePaused);
        }
    }
}
