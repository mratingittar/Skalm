using Skalm.Display;
using Skalm.GameObjects;

namespace Skalm.States
{
    internal class PlayerStateMachine : IStateMachine<PlayerStateBase, PlayerStates>
    {
        public PlayerStateBase CurrentState { get; private set; }
        private List<PlayerStateBase> _availableStates;
        private Player _player;
        private DisplayManager _displayManager;

        // CONSTRUCTOR I
        public PlayerStateMachine(Player player, DisplayManager displayManager, PlayerStates startingState)
        {
            _availableStates = new List<PlayerStateBase>();
            _player = player;
            _displayManager = displayManager;
            CurrentState = GetStateFromList(startingState);
        }

        // INITIALIZE STATE MACHINE
        public void Initialize(PlayerStates startingState)
        {
            CurrentState = GetStateFromList(startingState);
            CurrentState.Enter();
        }

        // CHANGE STATE MACHINE STATE
        public void ChangeState(PlayerStates newState)
        {
            CurrentState.Exit();
            CurrentState = GetStateFromList(newState);
            CurrentState.Enter();
        }

        // STATE MACHINE METHODS
        private PlayerStateBase GetStateFromList(PlayerStates newState)
        {
            return _availableStates.Find(state => state.GetType().Name == newState.ToString()) ?? CreateState(newState.ToString());
        }

        private PlayerStateBase CreateState(string stateName)
        {
            PlayerStateBase state;

            switch (stateName)
            {
                case "PlayerStateIdle":
                    state = new PlayerStateIdle(_player);
                    break;
                case "PlayerStateInteract":
                    state = new PlayerStateInteract(_player, _displayManager);
                    break;
                case "PlayerStateMenu":
                    state = new PlayerStateMenu(_player);
                    break;
                case "PlayerStateMessage":
                    state = new PlayerStateMessage(_player, _displayManager);
                    break;
                case "PlayerStateMove":
                    state = new PlayerStateMove(_player);
                    break;
                default:
                    state = new PlayerStateIdle(_player);
                    break;
            }

            _availableStates.Add(state);
            return state;
        }
    }
    internal enum PlayerStates
    {
        PlayerStateIdle,
        PlayerStateInteract,
        PlayerStateMenu,
        PlayerStateMessage,
        PlayerStateMove
    }
}
