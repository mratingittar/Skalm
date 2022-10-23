using Skalm.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
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
