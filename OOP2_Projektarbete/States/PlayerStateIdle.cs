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
    internal class PlayerStateIdle : PlayerStateBase
    {
        public PlayerStateIdle(Player player) : base(player) { }

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
        }

    }
}
