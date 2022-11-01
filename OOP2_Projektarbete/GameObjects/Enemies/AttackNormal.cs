using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Stats;
using Skalm.Structs;


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

                // DEAL DAMAGE ON DEFENDER STATS OBJECT
                statsDfn.TakeDamage(damage);

                // CREATE ATTACK MESSAGE
                if (damage.isCritical)
                    outputMsg += $"{statsAtk.name} deals a Critical Hit to {statsDfn.name}, dealing {damage.damage} damage!!! ";
                else
                    outputMsg += $"{statsAtk.name} hits {statsDfn.name}, dealing {damage.damage} damage! ";

                // DISPLAY REMAINING HP
                if (statsDfn.GetCurrentHP() > 0)
                    outputMsg += $"{statsDfn.name} has {statsDfn.GetCurrentHP()}/{statsDfn.GetMaxHP()} hp left...";
                else
                {
                    // CHECK FOR DEFENDER DEATH
                    outputMsg += $"{statsDfn.name} is dead. +{statsDfn.Experience} xp";
                    statsAtk.IncreaseExperience(statsDfn.Experience);
                }
            }
            else
            {
                // DISPLAY MISS MESSAGE
                outputMsg = $"{statsAtk.name} missed {statsDfn.name}...";
            }

            return outputMsg;
        }

    }
}
