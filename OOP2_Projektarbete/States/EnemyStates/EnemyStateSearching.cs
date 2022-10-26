using Skalm.GameObjects.Enemies;

namespace Skalm.States.EnemyStates
{
    internal class EnemyStateSearching : IEnemyState
    {
        private Enemy enemy;

        public EnemyStateSearching(Enemy enemy)
        {
            this.enemy = enemy;
        }
        public void Enter()
        {
            enemy.SetMoveBehaviour(EnemyMoveBehaviours.Pathfinding);
        }

        public void Exit()
        {

        }
    }
}
