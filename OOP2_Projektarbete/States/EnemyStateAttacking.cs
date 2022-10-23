using Skalm.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
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
