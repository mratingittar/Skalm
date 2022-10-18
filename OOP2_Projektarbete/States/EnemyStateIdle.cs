using Skalm.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class EnemyStateIdle : EnemyStateBase
    {
        private Enemy enemy;

        public EnemyStateIdle(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            enemy.SetMoveBehaviour(EnemyMoveBehaviours.Idle);
        }

        public override void Exit()
        {
            
        }
    }
}
