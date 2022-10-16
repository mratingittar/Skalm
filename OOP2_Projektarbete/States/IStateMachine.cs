using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal interface IStateMachine<T, in TIn> where T : IState
    {
        T CurrentState { get; }
        void Initialize(TIn startingState);
        void ChangeState(TIn newState);
    }
}
