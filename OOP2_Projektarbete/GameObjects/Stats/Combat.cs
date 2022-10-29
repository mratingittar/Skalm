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
            double baseDamage = statsAtk.statsArr[(int)EStats.BaseDamage].GetValue();
            bool isCritical = false;

            // STRENGTH BONUS
            double strBase = statsAtk.statsArr[(int)EStats.Strength].GetValue();
            double dmgStrBonus = strBase / 15;

            // DEXTERITY BONUS
            double dexBase = statsAtk.statsArr[(int)EStats.Dexterity].GetValue();
            double dmgDexBonus = dexBase / 20;

            // LUCK BONUS
            double lucBase = statsAtk.statsArr[(int)EStats.Luck].GetValue();
            double dmgLucBonus = lucBase / 10;

            // ROLLS
            int dmgRolls = 1 + (int)Math.Max(dmgStrBonus, dmgDexBonus);
            int dmgSides = 4 + (int)Math.Max(dmgStrBonus, dmgDexBonus);

            // BASE DAMAGE ROLL
            baseDamage += Dice.Rolls(dmgRolls, dmgSides);

            // CRITICAL HIT ROLL
            int critSides = 16 + (int)Math.Round(dmgLucBonus / 5);
            int critRolls = 1 + (int)(dmgDexBonus / 5) + (int)(dmgLucBonus / 5);
            int critMinRoll = 16;
            if (Dice.Chance(critMinRoll, critRolls, critSides))
            {
                isCritical = true;
                double critDmgMultiplier = 1.5 + (dexBase / 25) + (lucBase / 15);
                baseDamage = Math.Round(baseDamage * critDmgMultiplier);
            }

            // ARMOR CALCULATIONS
            double armorBase = statsDfn.statsArr[(int)EStats.Armor].GetValue();

            // LINEAR
            int hardArmorRed = rng.Next(1, (int)Math.Ceiling(armorBase / 2));
            baseDamage -= hardArmorRed;

            // PERCENTAGES
            double baseArmorRed = 1 - (0.015625 * armorBase);
            double extraArmorRed = 1 - (0.03125 * armorBase * (rng.NextDouble() + 0.00000001));
            baseDamage *= (baseArmorRed * extraArmorRed);

            // CREATE DAMAGE OBJECT
            return new DoDamage() { damage = Math.Max(1, (int)Math.Round(baseDamage)), originXY = Vector2Int.Zero, isCritical = isCritical };
        }
    }
}
