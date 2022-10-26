using Skalm.GameObjects.Enemies;

namespace Skalm.States.EnemyStates
{
    internal class EnemyStatePatrolling : IEnemyState
    {
        private Enemy enemy;

        public EnemyStatePatrolling(Enemy enemy)
        {
            this.enemy = enemy;
        }
        public void Enter()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }
    }
}
