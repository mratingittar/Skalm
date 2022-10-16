using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameManagerStateMachine : IStateMachine<IGameState, GameStates>
    {
        public IGameState CurrentState { get; private set; }
        private List<IGameState> availableStates;
        private GameManager gameManager;

        // CONSTRUCTOR I
        public GameManagerStateMachine(GameManager gameManager, GameStates startingState)
        {
            this.gameManager = gameManager;
            availableStates = new List<IGameState>();
            CurrentState = GetStateFromList(startingState);
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
            return availableStates.Find(state => state.GetType().Name == newState.ToString()) ?? CreateState(newState.ToString());
        }

        private IGameState CreateState(string stateName)
        {
            IGameState state;
            switch (stateName)
            {
                case "GameStateInitializing":
                    state = new GameStateInitializing(gameManager);
                    break;
                case "GameStateMainMenu":
                    state = new GameStateMainMenu(gameManager);
                        break;
                case "GameStatePaused":
                    state = new GameStatePaused(gameManager);
                    break;
                case "GameStatePlaying":
                    state = new GameStatePlaying(gameManager);
                    break;
                default:
                    state = new GameStateInitializing(gameManager);
                    break;
            }
            availableStates.Add(state);
            return state;
        }
    }
    internal enum GameStates
    {
        GameStateInitializing,
        GameStateMainMenu,
        GameStatePaused,
        GameStatePlaying
    }
}
