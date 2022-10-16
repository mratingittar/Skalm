using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class PlayerStateMove : IPlayerState
    {
        public GameManager GameManager { get; private set; }

        public PlayerStateMove(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        public void Enter()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }
    }
}
