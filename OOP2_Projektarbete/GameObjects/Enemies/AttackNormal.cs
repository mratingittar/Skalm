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
        public string Attack(ActorStatsObject statsAtk, ActorStatsObject statsDfn)
        {
            string outMsg = "";

            // CHECK FOR HIT OR MISS
            if (Combat.ToHitCalc(statsAtk.stats, statsDfn.stats))
            {
                DoDamage damage = Combat.DamageCalc(statsAtk.stats, statsDfn.stats);
                damage.sender = statsAtk;
                damage.originXY = Vector2Int.Zero;

                statsDfn.TakeDamage(damage);
            }

            return outMsg;
        }

    }
}
