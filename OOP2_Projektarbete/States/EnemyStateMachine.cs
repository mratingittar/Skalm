using Skalm.GameObjects;
using Skalm.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class EnemyStateMachine : IStateMachine<IEnemyState, EnemyStates>
    {

        private Enemy enemy;
        private List<IEnemyState> availableStates;
        
        public IEnemyState CurrentState { get; private set; }

        public EnemyStateMachine(Enemy enemy, EnemyStates startingState)
        {
            this.enemy = enemy;
            availableStates = new List<IEnemyState>();
            CurrentState = GetStateFromList(startingState);
        }


        // INITIALIZE STATE MACHINE
        public void Initialize(EnemyStates startingState)
        {
            CurrentState = GetStateFromList(startingState);
            CurrentState.Enter();
        }

        // CHANGE STATE MACHINE STATE
        public void ChangeState(EnemyStates newState)
        {
            CurrentState.Exit();
            CurrentState = GetStateFromList(newState);
            CurrentState.Enter();
        }

        // STATE MACHINE METHODS
        private IEnemyState GetStateFromList(EnemyStates newState)
        {
            return availableStates.Find(state => state.GetType().Name == newState.ToString()) ?? CreateState(newState.ToString());
        }

        private IEnemyState CreateState(string stateName)
        {
            IEnemyState state;

            switch (stateName)
            {
                case "EnemyStateIdle":
                    state = new EnemyStateIdle(enemy);
                    break;
                case "EnemyStatePatrolling":
                    state = new EnemyStatePatrolling(enemy);
                    break;
                case "EnemyStateSearching":
                    state = new EnemyStateSearching(enemy);
                    break;
                case "EnemyStateAttacking":
                    state = new EnemyStateAttacking(enemy);
                    break;
                default:
                    state = new EnemyStateIdle(enemy);
                    break;
            }

            availableStates.Add(state);
            return state;
        }
    }


    internal enum EnemyStates
    {
        EnemyStateIdle,
        EnemyStatePatrolling,
        EnemyStateSearching,
        EnemyStateAttacking
    }
}
