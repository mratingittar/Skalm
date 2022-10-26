using Skalm.GameObjects.Enemies;

namespace Skalm.States.EnemyStates
{
    internal class EnemyStateAttacking : IEnemyState
    {
        private Enemy enemy;

        public EnemyStateAttacking(Enemy enemy)
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
