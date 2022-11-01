
using Skalm.Input;
using Skalm.Structs;


namespace Skalm.States.PlayerStates
{
    internal interface IPlayerState : IState
    {
        //METHOD MOVE INPUT
        public abstract void MoveInput(Vector2Int direction);

        // METHOD COMMAND INPUT
        public abstract void CommandInput(InputCommands command);
    }
}
