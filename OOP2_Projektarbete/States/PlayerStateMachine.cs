using Skalm.GameObjects;

namespace Skalm.States
{
    internal class PlayerStateMachine : IStateMachine<PlayerStateBase, PlayerStates>
    {
        private Player player;
        private List<PlayerStateBase> availableStates;
        public PlayerStateBase CurrentState { get; private set; }

        // CONSTRUCTOR I
        public PlayerStateMachine(Player player, PlayerStates startingState)
        {
            this.player = player;
            availableStates = new List<PlayerStateBase>();
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
            return availableStates.Find(state => state.GetType().Name == newState.ToString()) ?? CreateState(newState.ToString());
        }

        private PlayerStateBase CreateState(string stateName)
        {
            PlayerStateBase state;

            switch (stateName)
            {
                case "PlayerStateIdle":
                    state = new PlayerStateIdle(player);
                    break;
                case "PlayerStateMove":
                    state = new PlayerStateMove(player);
                    break;
                case "PlayerStateAttack":
                    state = new PlayerStateAttack(player);
                    break;
                case "PlayerStateLook":
                    state = new PlayerStateLook(player);
                    break;
                case "PlayerStateMenu":
                    state = new PlayerStateMenu(player);
                    break;
                default:
                    state = new PlayerStateIdle(player);
                    break;
            }

            availableStates.Add(state);
            return state;
        }
    }
    internal enum PlayerStates
    {
        PlayerStateIdle,
        PlayerStateMove,
        PlayerStateAttack,
        PlayerStateLook,
        PlayerStateMenu
    }
}
