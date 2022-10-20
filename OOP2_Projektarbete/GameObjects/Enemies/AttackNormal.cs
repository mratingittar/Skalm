using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Stats;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Enemies
{
    internal class AttackNormal : IAttackComponent
    {
        public DoDamage Attack()
        {
            return new DoDamage();
        }

        public void Attack(ActorStatsObject statsAtk, ActorStatsObject statsDfn)
        {
            throw new NotImplementedException();
        }
    }
}
