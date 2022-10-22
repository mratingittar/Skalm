using Skalm.GameObjects.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal static class ItemGen
    {
        static readonly Random rng = new Random();

        // GET RANDOM ITEM FROM WEIGHTED LIST
        public static T WeightedRandomFromList<T>(List<(float, T)> inList)
        {
            float sumTotal = 0;
            float sumRng = 0;
            float choice;

            // GET SUM OF ALL ITEM WEIGHTS
            foreach (var item in inList)
            {
                sumTotal += (item.Item1);
            }

            // RANDOMIZE CHOICE
            choice = (float)rng.NextDouble() * sumTotal;

            // ITERATE LIST & PICK RANDOM ITEM
            for (int i = 0; i < inList.Count; i++)
            {
                sumRng += inList[i].Item1;

                // PICK ITEM IF RNG VALUE EXCEEDED
                if ((choice > (sumRng - inList[i].Item1))
                && (choice < sumRng))
                    return inList[i].Item2;
            }

            // UGLY DEFAULT
            return inList[0].Item2;
        }

        // GENERATE RANDOM EQUIPMENT
        public static ItemEquippable GetRandomEquippable(float bonusMod = 0.5f, int eqSlot = -1)
        {
            string itemName = "";
            int bonusCounter = 0;
            double addBonusChance = 1;

            // BASE BONUS ITEMS
            StatsObject itemStats = new StatsObject();
            List<(float, EStats)> bonusList = new List<(float, EStats)>();

            var strBonus = (0.1f, EStats.Strength);
            var dexBonus = (0.1f, EStats.Dexterity);
            var conBonus = (0.1f, EStats.Constitution);
            var intBonus = (0.1f, EStats.Intelligence);
            var lucBonus = (0.1f, EStats.Luck);
            var hpBonus = (0.1f, EStats.HP);
            var dmgBonus = (0.1f, EStats.BaseDamage);
            var armBonus = (0.1f, EStats.Armor);

            // RANDOMIZE EQUIP SLOT?
            if (eqSlot == -1)
                eqSlot = rng.Next(0, Enum.GetValues(typeof(EEqSlots)).Length);

            // BASE ITEM TYPE ATTRIBUTES
            switch(eqSlot)
            {
                // HEAD
                case (int)EEqSlots.Head:
                    itemName = "Helmet";
                    itemStats.statsArr[(int)EStats.Armor].SetValue(1);
                    armBonus.Item1 *= 10f;
                    hpBonus.Item1 *= 5f;
                    conBonus.Item1 *= 3f;
                    break;

                // WEAPON
                case (int)EEqSlots.RHand:
                    itemName = "Sword";
                    itemStats.statsArr[(int)EStats.BaseDamage].SetValue(1);
                    strBonus.Item1 *= 15f;
                    dexBonus.Item1 *= 15f;
                    dmgBonus.Item1 *= 25f;
                    break;

                // SHIELD
                case (int)EEqSlots.LHand:
                    itemName = "Shield";
                    itemStats.statsArr[(int)EStats.Armor].SetValue(1);
                    armBonus.Item1 *= 25f;
                    strBonus.Item1 *= 5f;
                    dexBonus.Item1 *= 5f;
                    break;

                // BODY
                case (int)EEqSlots.Torso:
                    itemName = "Armor";
                    itemStats.statsArr[(int)EStats.Armor].SetValue(2);
                    armBonus.Item1 *= 50f;
                    hpBonus.Item1 *= 25f;
                    conBonus.Item1 *= 25f;
                    break;

                // FEET
                case (int)EEqSlots.Feet:
                    itemName = "Boots";
                    itemStats.statsArr[(int)EStats.Armor].SetValue(1);
                    armBonus.Item1 *= 15f;
                    dexBonus.Item1 *= 5f;
                    lucBonus.Item1 *= 3f;
                    break;

                // RIGHT FINGER
                case (int)EEqSlots.RFinger:
                    itemStats.statsArr[rng.Next(0, Enum.GetValues(typeof(EStats)).Length)].SetValue(1);
                    itemName = "Ring";
                    break;

                // LEFT FINGER
                case (int)EEqSlots.LFinger:
                    itemStats.statsArr[rng.Next(0, Enum.GetValues(typeof(EStats)).Length)].SetValue(1);
                    itemName = "Ring";
                    break;

                // DEFAULT TO RING
                default:
                    eqSlot = (int)EEqSlots.RFinger;
                    itemStats.statsArr[rng.Next(0, Enum.GetValues(typeof(EStats)).Length)].SetValue(1);
                    itemName = "Ring";
                    break;
            }

            // ADD BONUS ITEMS TO LIST
            bonusList.Add(strBonus);
            bonusList.Add(dexBonus);
            bonusList.Add(conBonus);
            bonusList.Add(intBonus);
            bonusList.Add(lucBonus);
            bonusList.Add(hpBonus);
            bonusList.Add(dmgBonus);
            bonusList.Add(armBonus);

            // RANDOMIZE BONUS COUNT
            do
            {
                bonusCounter++;
                addBonusChance *= bonusMod;
            } while (rng.NextDouble() < addBonusChance);

            // ADD BONUS STATS TO ITEM STATS OBJECT
            for (int i = 0; i < bonusCounter; i++)
            {
                EStats bonus = WeightedRandomFromList(bonusList);
                itemStats.statsArr[(int)bonus].AddModifier(1);
            }

            // UPDATE ITEM NAME
            itemName += $" +{bonusCounter}";

            // CREATE & RETURN ITEM
            return new ItemEquippable(itemName, eqSlot, itemStats);
        }
    }
}
