using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal interface IGameState
    {
        void Enter();
        void Exit();
    }
}
