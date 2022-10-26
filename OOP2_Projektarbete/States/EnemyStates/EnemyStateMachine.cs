using Skalm.GameObjects;
using Skalm.GameObjects.Enemies;

namespace Skalm.States.EnemyStates
{
    internal class EnemyStateMachine : IStateMachine<IEnemyState, EEnemyStates>
    {

        private Enemy enemy;
        private List<IEnemyState> availableStates;

        public IEnemyState CurrentState { get; private set; }

        public EnemyStateMachine(Enemy enemy, EEnemyStates startingState)
        {
            this.enemy = enemy;
            availableStates = new List<IEnemyState>();
            CurrentState = GetStateFromList(startingState);
        }


        // INITIALIZE STATE MACHINE
        public void Initialize(EEnemyStates startingState)
        {
            CurrentState = GetStateFromList(startingState);
            CurrentState.Enter();
        }

        // CHANGE STATE MACHINE STATE
        public void ChangeState(EEnemyStates newState)
        {
            CurrentState.Exit();
            CurrentState = GetStateFromList(newState);
            CurrentState.Enter();
        }

        // STATE MACHINE METHODS
        private IEnemyState GetStateFromList(EEnemyStates newState)
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


    internal enum EEnemyStates
    {
        EnemyStateIdle,
        EnemyStatePatrolling,
        EnemyStateSearching,
        EnemyStateAttacking
    }
}
