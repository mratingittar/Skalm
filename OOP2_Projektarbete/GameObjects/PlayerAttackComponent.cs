using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Stats;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects
{
    internal class PlayerAttackComponent : IAttackComponent
    {
        Player player;

        // CONSTRUCTOR I
        public PlayerAttackComponent(Player player)
        {
            this.player = player;
        }

        // ATTACK
        public void Attack(ActorStatsObject statsAtk, ActorStatsObject statsDfn)
        {
            // CHECK FOR HIT OR MISS
            if (Combat.ToHitCalc(statsAtk.stats, statsDfn.stats))
            {
                DoDamage damage = Combat.DamageCalc(statsAtk.stats, statsDfn.stats);
                damage.sender = statsAtk;
                damage.originXY = player.GridPosition;

                statsDfn.TakeDamage(damage);
            }
        }
    }
}
