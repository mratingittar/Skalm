namespace Skalm.States
{
    internal interface IGameState : IState
    {
        void UpdateLogic();
        void UpdateDisplay();
    }
}
