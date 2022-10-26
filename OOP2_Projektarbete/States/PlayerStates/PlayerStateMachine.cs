using Skalm.Display;
using Skalm.GameObjects;
using Skalm.Maps;

namespace Skalm.States.PlayerStates
{
    internal class PlayerStateMachine : IStateMachine<IPlayerState, EPlayerStates>
    {
        public IPlayerState CurrentState { get; private set; }
        private List<IPlayerState> _availableStates;
        private Player _player;
        private DisplayManager _displayManager;
        private MapManager _mapManager;

        // CONSTRUCTOR I
        public PlayerStateMachine(Player player, DisplayManager displayManager, MapManager mapManager, EPlayerStates startingState)
        {
            _availableStates = new List<IPlayerState>();
            _player = player;
            _displayManager = displayManager;
            _mapManager = mapManager;
            CurrentState = GetStateFromList(startingState);
        }

        // INITIALIZE STATE MACHINE
        public void Initialize(EPlayerStates startingState)
        {
            CurrentState = GetStateFromList(startingState);
            CurrentState.Enter();
        }

        // CHANGE STATE MACHINE STATE
        public void ChangeState(EPlayerStates newState)
        {
            CurrentState.Exit();
            CurrentState = GetStateFromList(newState);
            CurrentState.Enter();
        }

        // STATE MACHINE METHODS
        private IPlayerState GetStateFromList(EPlayerStates newState)
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
    internal enum EPlayerStates
    {
        PlayerStateIdle,
        PlayerStateInteract,
        PlayerStateMenu,
        PlayerStateMessage,
        PlayerStateMove
    }
}
