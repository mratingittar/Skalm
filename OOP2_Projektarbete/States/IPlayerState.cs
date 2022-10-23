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
    internal interface IPlayerState : IState
    {
        //METHOD MOVE INPUT
        public abstract void MoveInput(Vector2Int direction);

        // METHOD COMMAND INPUT
        public abstract void CommandInput(InputCommands command);
    }
}
