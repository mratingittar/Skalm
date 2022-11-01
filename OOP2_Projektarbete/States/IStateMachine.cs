

namespace Skalm.States
{
    internal interface IStateMachine<T, in TIn> where T : IState
    {
        T CurrentState { get; }
        void Initialize(TIn startingState);
        void ChangeState(TIn newState);
    }
}
