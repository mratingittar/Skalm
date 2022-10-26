using Skalm.States.PlayerStates;

namespace Skalm.States.GameStates
{
    internal class GameStateMachine : IStateMachine<IGameState, EGameStates>
    {
        public IGameState CurrentState { get; private set; }

        private List<IGameState> _availableStates;
        private GameManager _gameManager;

        // CONSTRUCTOR I
        public GameStateMachine(GameManager gameManager, EGameStates startingState)
        {
            _gameManager = gameManager;
            _availableStates = new List<IGameState>();
            CurrentState = GetStateFromList(startingState);
            PlayerStateMove.OnPauseMenuRequested += PauseGame;
        }

        private void PauseGame()
        {
            ChangeState(EGameStates.GameStatePaused);
        }

        // INITIALIZE STATE MACHINE
        public void Initialize(EGameStates initialState)
        {
            CurrentState = GetStateFromList(initialState);
            CurrentState.Enter();
        }

        // CHANGE STATE MACHINE STATE
        public void ChangeState(EGameStates newState)
        {
            CurrentState.Exit();
            CurrentState = GetStateFromList(newState);
            CurrentState.Enter();
        }

        // STATE MACHINE METHODS
        private IGameState GetStateFromList(EGameStates newState)
        {
            return _availableStates.Find(state => state.GetType().Name == newState.ToString()) ?? CreateState(newState.ToString());
        }

        private IGameState CreateState(string stateName)
        {
            IGameState state;
            switch (stateName)
            {
                case "GameStateInitializing":
                    state = new GameStateInitializing(_gameManager);
                    break;
                case "GameStateMainMenu":
                    state = new GameStateMainMenu(_gameManager);
                    break;
                case "GameStatePaused":
                    state = new GameStatePaused(_gameManager);
                    break;
                case "GameStatePlaying":
                    state = new GameStatePlaying(_gameManager);
                    break;
                case "GameStateGameOver":
                    state = new GameStateGameOver(_gameManager);
                    break;
                default:
                    state = new GameStateInitializing(_gameManager);
                    break;
            }
            _availableStates.Add(state);
            return state;
        }
    }
    internal enum EGameStates
    {
        GameStateInitializing,
        GameStateMainMenu,
        GameStatePaused,
        GameStatePlaying,
        GameStateGameOver
    }
}
