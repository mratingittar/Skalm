using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStateMachine : IStateMachine<GameStateBase, GameStates>
    {
        public GameStateBase CurrentState { get; private set; }
        private List<GameStateBase> availableStates;
        private GameManager gameManager;

        // CONSTRUCTOR I
        public GameStateMachine(GameManager gameManager, GameStates startingState)
        {
            this.gameManager = gameManager;
            availableStates = new List<GameStateBase>();
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
        private GameStateBase GetStateFromList(GameStates newState)
        {
            return availableStates.Find(state => state.GetType().Name == newState.ToString()) ?? CreateState(newState.ToString());
        }

        private GameStateBase CreateState(string stateName)
        {
            GameStateBase state;
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
                case "GameStateGameOver":
                    state = new GameStateGameOver(gameManager);
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
        GameStatePlaying,
        GameStateGameOver
    }
}
