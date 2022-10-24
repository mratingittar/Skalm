using Skalm.Display;
using Skalm.GameObjects;
using Skalm.Map;

namespace Skalm.States
{
    internal class PlayerStateMachine : IStateMachine<IPlayerState, PlayerStates>
    {
        public IPlayerState CurrentState { get; private set; }
        private List<IPlayerState> _availableStates;
        private Player _player;
        private DisplayManager _displayManager;
        private MapManager _mapManager;

        // CONSTRUCTOR I
        public PlayerStateMachine(Player player, DisplayManager displayManager, MapManager mapManager, PlayerStates startingState)
        {
            _availableStates = new List<IPlayerState>();
            _player = player;
            _displayManager = displayManager;
            _mapManager = mapManager;
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
        private IPlayerState GetStateFromList(PlayerStates newState)
        {
            return _availableStates.Find(state => state.GetType().Name == newState.ToString()) ?? CreateState(newState.ToString());
        }

        private IPlayerState CreateState(string stateName)
        {
            IPlayerState state;

            switch (stateName)
            {
                case "PlayerStateIdle":
                    state = new PlayerStateIdle();
                    break;
                case "PlayerStateInteract":
                    state = new PlayerStateInteract(_player, _displayManager, _mapManager);
                    break;
                case "PlayerStateMenu":
                    state = new PlayerStateMenu(_player, _displayManager);
                    break;
                case "PlayerStateMessage":
                    state = new PlayerStateMessage(_player, _displayManager);
                    break;
                case "PlayerStateMove":
                    state = new PlayerStateMove(_player);
                    break;
                default:
                    state = new PlayerStateIdle();
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
