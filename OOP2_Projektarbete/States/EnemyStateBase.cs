using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal abstract class EnemyStateBase : IState
    {
        // ENTER STATE
        public abstract void Enter();

        // EXIT STATE
        public abstract void Exit();
    }
}
