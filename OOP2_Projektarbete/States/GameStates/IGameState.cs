namespace Skalm.States.GameStates
{
    internal interface IGameState : IState
    {
        void UpdateLogic();
        void UpdateDisplay();
    }
}
