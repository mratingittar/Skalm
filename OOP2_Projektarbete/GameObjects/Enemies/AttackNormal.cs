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
            string outputMsg = "";

            // CHECK FOR HIT OR MISS
            if (Combat.ToHitCalc(statsAtk.stats, statsDfn.stats))
            {
                DoDamage damage = Combat.DamageCalc(statsAtk.stats, statsDfn.stats);
                damage.sender = statsAtk;
                damage.originXY = Vector2Int.Zero;

                statsDfn.TakeDamage(damage);

                outputMsg = $"{statsAtk.name} hit {statsDfn.name}, dealing {damage.damage} damage! ";
                if (statsDfn.GetCurrentHP() > 0)
                    outputMsg += $"{statsDfn.name} has {statsDfn.GetCurrentHP()}/{statsDfn.stats.statsArr[(int)EStats.HP].GetValue()} hp left...";
                else
                    outputMsg += $"{statsDfn.name} is dead.";
            }
            else
            {
                outputMsg = $"{statsAtk.name} missed {statsDfn.name}...";
            }

            return outputMsg;
        }

    }
}
