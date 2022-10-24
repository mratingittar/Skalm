using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStateMachine : IStateMachine<IGameState, GameStates>
    {
        public IGameState CurrentState { get; private set; }

        private List<IGameState> _availableStates;
        private GameManager _gameManager;

        // CONSTRUCTOR I
        public GameStateMachine(GameManager gameManager, GameStates startingState)
        {
            this._gameManager = gameManager;
            _availableStates = new List<IGameState>();
            CurrentState = GetStateFromList(startingState);
            PlayerStateMove.OnPauseMenuRequested += PauseGame;
        }

        private void PauseGame()
        {
            ChangeState(GameStates.GameStatePaused);
        }

        // INITIALIZE STATE MACHINE
        public void Initialize(GameStates initialState)
        {
            CurrentState = GetStateFromList(initialState);
            CurrentState.Enter();
        }

        // CHANGE STATE MACHINE STATE
        public void ChangeState(GameStates newState)
        {
            CurrentState.Exit();
            CurrentState = GetStateFromList(newState);
            CurrentState.Enter();
        }

        // STATE MACHINE METHODS
        private IGameState GetStateFromList(GameStates newState)
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
    internal enum GameStates
    {
        GameStateInitializing,
        GameStateMainMenu,
        GameStatePaused,
        GameStatePlaying,
        GameStateGameOver
    }
}
