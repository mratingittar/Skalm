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
    internal abstract class PlayerStateBase : IState
    {
        protected GameManager gameManager;
        protected Player player;

        public PlayerStateBase(GameManager gameManager, Player player)
        {
            this.gameManager = gameManager;
            this.player = player;
        }

        // ENTER STATE
        public abstract void Enter();

        // EXIT STATE
        public abstract void Exit();

        //METHOD MOVE INPUT
        public abstract void MoveInput(Vector2Int direction);

        // METHOD COMMAND INPUT
        public abstract void CommandInput(InputCommands command);
    }
}
