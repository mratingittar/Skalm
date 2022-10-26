using Skalm.GameObjects.Enemies;

namespace Skalm.States.EnemyStates
{
    internal class EnemyStateIdle : IEnemyState
    {
        private Enemy enemy;

        public EnemyStateIdle(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            enemy.SetMoveBehaviour(EnemyMoveBehaviours.Idle);
        }

        public void Exit()
        {

        }
    }
}
