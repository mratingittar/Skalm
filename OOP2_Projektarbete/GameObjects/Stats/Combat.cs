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
            // DEXTERITY BONUS
            float dexValue = statsAtk.statsArr[(int)EStats.Dexterity].GetValue();
            float dexBonusToHit = dexValue / 10;

            // LUCK BONUS
            float lucValue = statsAtk.statsArr[(int)EStats.Luck].GetValue();
            float lucBonusToHit = (int)Math.Round(lucValue / 15);

            // TO HIT ROLLS
            int rollsTH = 1 + (int)Math.Round(dexBonusToHit + lucBonusToHit);
            int dieSidesTH = 6 + (int)Math.Round(dexBonusToHit + lucBonusToHit);

            return Dice.Chance(4, rollsTH, dieSidesTH);
        }

        // CALCULATE ATTACK DAMAGE METHOD
        public static DoDamage DamageCalc(StatsObject statsAtk, StatsObject statsDfn)
        {
            float baseDamage = statsAtk.statsArr[(int)EStats.BaseDamage].GetValue();
            bool isCritical = false;

            // STRENGTH BONUS
            float strBase = statsAtk.statsArr[(int)EStats.Strength].GetValue();
            float dmgStrBonus = (int)Math.Round((float)strBase / 15);

            // DEXTERITY BONUS
            float dexBase = statsAtk.statsArr[(int)EStats.Dexterity].GetValue();
            float dmgDexBonus = (int)Math.Round(dexBase / 20);

            // LUCK BONUS
            float lucBase = statsAtk.statsArr[(int)EStats.Luck].GetValue();
            float dmgLucBonus = (int)Math.Round(lucBase / 10);

            // ROLLS
            int dmgRollsBonus = 1 + (int)Math.Max(dmgStrBonus, dmgDexBonus);
            int dmgSidesBonus = 3 + (int)Math.Max(dmgStrBonus, dmgDexBonus);

            // BASE DAMAGE ROLL
            baseDamage += Dice.Rolls(dmgRollsBonus, dmgSidesBonus);

            // CRITICAL HIT ROLL
            int critSides = 16 + (int)Math.Round(dmgLucBonus / 5);
            int critRolls = 1 + (int)(dmgDexBonus / 5) + (int)(dmgLucBonus / 5);
            int critMinRoll = 16;
            if (Dice.Chance(critMinRoll, critRolls, critSides))
            {
                isCritical = true;
                float critDmgMultiplier = 1 + (dexBase / 25) + (lucBase / 15);
                baseDamage = (int)Math.Round(baseDamage * critDmgMultiplier);
            }

            // CREATE DAMAGE OBJECT
            return new DoDamage() { damage = baseDamage, originXY = Vector2Int.Zero, isCritical = isCritical };
        }
    }
}
