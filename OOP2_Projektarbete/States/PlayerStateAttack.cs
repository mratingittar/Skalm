using Skalm.Actors;
using Skalm.Input;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class PlayerStateAttack : PlayerStateBase
    {
        public PlayerStateAttack(GameManager gameManager, Player player) : base(gameManager, player) { }


        public override void Enter()
        {
            throw new NotImplementedException();
        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }

        public override void MoveInput(Vector2Int direction)
        {
            throw new NotImplementedException();
        }
        public override void CommandInput(InputCommands command)
        {
            throw new NotImplementedException();
        }
    }
}
