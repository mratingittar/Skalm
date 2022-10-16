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

        public PlayerStateBase(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public abstract void Enter();

        public abstract void Exit();
    }
}
