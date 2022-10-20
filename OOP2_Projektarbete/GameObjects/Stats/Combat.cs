using Skalm.Structs;
using Skalm.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal static class Combat
    {
        static readonly Random rng = new Random();

        // CALCULATE HIT OR MISS
        public static bool ToHitCalc(StatsObject statsAtk, StatsObject statsDfn)
        {
            float baseToHit = statsAtk.statsArr[(int)EStats.Dexterity].GetValue();
            int bonusToHit = (int)Math.Round(baseToHit / 10);
            return (Dice.Chance(4 - bonusToHit, 1 + bonusToHit));
        }

        // CALCULATE ATTACK DAMAGE METHOD
        public static DoDamage DamageCalc(StatsObject statsAtk, StatsObject statsDfn)
        {
            float outputDamage = statsAtk.statsArr[(int)EStats.BaseDamage].GetValue();
            float baseToHit = statsAtk.statsArr[(int)EStats.Dexterity].GetValue();

            int damageStrBonus = (int)Math.Round((float)statsAtk.statsArr[(int)EStats.Strength].GetValue() / 10);
            int bonusToHit = (int)Math.Round(baseToHit / 10);
            int critRollsBonus = bonusToHit / 5;

            // BASE DAMAGE ROLL
            outputDamage += Dice.Rolls(1 + damageStrBonus, 6 + damageStrBonus);

            // CRITICAL HIT ROLL
            if (Dice.Chance(12 - bonusToHit, 1 + critRollsBonus, 12 - bonusToHit))
            {
                float critDmgMultiplier = 1 + (baseToHit / 15);
                outputDamage = (int)Math.Round(outputDamage * critDmgMultiplier);
            }

            // CREATE DAMAGE OBJECT
            return new DoDamage() { damage = outputDamage, originXY = Vector2Int.Zero };
        }
    }
}
