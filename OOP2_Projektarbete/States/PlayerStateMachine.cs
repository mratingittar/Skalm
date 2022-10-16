namespace Skalm.States
{
    internal class PlayerStateMachine : IStateMachine<IPlayerState, PlayerStates>
    {
        public IPlayerState CurrentState { get; private set; }
        private List<IPlayerState> availableStates;
        private GameManager gameManager;

        // CONSTRUCTOR I
        public PlayerStateMachine(GameManager gameManager, PlayerStates startingState)
        {
            this.gameManager = gameManager;
            availableStates = new List<IPlayerState>();
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
            return availableStates.Find(state => state.GetType().Name == newState.ToString()) ?? CreateState(newState.ToString());
        }

        private IPlayerState CreateState(string stateName)
        {
            IPlayerState state;

            switch (stateName)
            {
                case "PlayerStateIdle":
                    state = new PlayerStateIdle(gameManager);
                    break;
                case "PlayerStateMove":
                    state = new PlayerStateMove(gameManager);
                    break;
                case "PlayerStateAttack":
                    state = new PlayerStateAttack(gameManager);
                    break;
                case "PlayerStateLook":
                    state = new PlayerStateLook(gameManager);
                    break;
                case "PlayerStateMenu":
                    state = new PlayerStateMenu(gameManager);
                    break;
                default:
                    state = new PlayerStateIdle(gameManager);
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
