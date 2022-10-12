using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameManagerStateMachine
    {
        public IGameState CurrentState { get; private set; }

        // CONSTRUCTOR I
        public GameManagerStateMachine(IGameState currentState)
        {
            CurrentState = currentState;
        }

        // INITIALIZE STATE MACHINE
        public void Initialize(IGameState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        // CHANGE STATE MACHINE STATE
        public void ChangeState(IGameState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
