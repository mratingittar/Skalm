using Skalm.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class EnemyStateSearching : EnemyStateBase
    {
        private Enemy enemy;

        public EnemyStateSearching(Enemy enemy)
        {
            this.enemy = enemy;
        }
        public override void Enter()
        {
            enemy.SetMoveBehaviour(EnemyMoveBehaviours.Pathfinding);
        }

        public override void Exit()
        {
            
        }
    }
}
