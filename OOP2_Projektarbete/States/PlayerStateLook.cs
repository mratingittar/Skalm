using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class PlayerStateLook : IPlayerState
    {
        public GameManager GameManager { get; private set; }

        public PlayerStateLook(GameManager gameManager)
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
