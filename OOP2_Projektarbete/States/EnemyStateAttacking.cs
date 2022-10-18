using Skalm.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class EnemyStateAttacking : EnemyStateBase
    {
        private Enemy enemy;

        public EnemyStateAttacking(Enemy enemy)
        {
            this.enemy = enemy;
        }
        public override void Enter()
        {
            throw new NotImplementedException();
        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }
    }
}
