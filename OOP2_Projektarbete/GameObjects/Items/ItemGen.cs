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

        // GENERATE RANDOM POTION
        public static Potion GetRandomPotion(float bonusMod = 0.65f)
        {
            int healAmount = 5;
            int bonusCounter = 0;
            float addHealChance = 0.75f;
            string potionName = "Potion of Healing";

            // RANDOMIZE HEAL AMOUNT
            do
            {
                bonusCounter++;
                healAmount += rng.Next(1, 4);
                addHealChance *= bonusMod;
            } while (rng.NextDouble() < addHealChance);

            // UPDATE ITEM NAME
            if ((bonusCounter - 2) > 0)
                potionName += $" +{bonusCounter - 2}";

            // CREATE & RETURN POTION
            return new Potion(potionName, healAmount);
        }

        // GENERATE RANDOM EQUIPMENT
        public static ItemEquippable GetRandomEquippable(float bonusMod = 0.65f, int eqSlot = -1)
        {
            string itemName = "";
            int bonusCounter = 0;
            double addBonusChance = 1;

            // RANDOMIZE EQUIP SLOT?
            if (eqSlot == -1)
                eqSlot = rng.Next(0, Enum.GetValues(typeof(EEqSlots)).Length);

            // BASE BONUS ITEMS
            StatsObject itemStats = new StatsObject();

            // STATS BONUSES
            List<(float, (EStats, int))> statBonusList = new List<(float, (EStats, int))>();

            var strBonus = (0.1f, (EStats.Strength, 1));
            var dexBonus = (0.1f, (EStats.Dexterity, 1));
            var conBonus = (0.1f, (EStats.Constitution, 1));
            var intBonus = (0.1f, (EStats.Intelligence, 1));
            var lucBonus = (0.1f, (EStats.Luck, 1));
            var hpBonus = (0.1f, (EStats.HP, 2));
            var dmgBonus = (0.1f, (EStats.BaseDamage, 1));
            var armBonus = (0.1f, (EStats.Armor, 1));

            // MATERIAL BONUSES
            List<(float, (string, float))> materialBonusList = new List<(float, (string, float))>();

            materialBonusList.Add((0.65f, ("Rusty", 0.75f)));
            materialBonusList.Add((0.65f, ("Tattered", 0.75f)));
            materialBonusList.Add((0.65f, ("Bent", 0.85f)));
            materialBonusList.Add((1f, ("Bone", 0.9f)));
            materialBonusList.Add((1f, ("Worn", 0.9f)));
            materialBonusList.Add((1f, ("Wood", 1f)));
            materialBonusList.Add((1f, ("Average", 1f)));
            materialBonusList.Add((1f, ("Common", 1f)));
            materialBonusList.Add((1f, ("Rough", 1f)));
            materialBonusList.Add((1f, ("Stone", 1.1f)));
            materialBonusList.Add((0.8f, ("Leather", 1.25f)));
            materialBonusList.Add((0.8f, ("Studded", 1.45f)));
            materialBonusList.Add((0.8f, ("Fine", 1.45f)));
            materialBonusList.Add((0.8f, ("Polished", 1.75f)));
            materialBonusList.Add((0.7f, ("Bronze", 2.65f)));
            materialBonusList.Add((0.7f, ("Exceptional", 3.0f)));
            materialBonusList.Add((0.6f, ("Copper", 3.0f)));
            materialBonusList.Add((0.5f, ("Iron", 4.25f)));
            materialBonusList.Add((0.4f, ("Steel", 6.5f)));
            materialBonusList.Add((0.3f, ("Etherium", 11.5f)));

            // RANDOMIZE MATERIAL
            var itemMaterial = WeightedRandomFromList(materialBonusList);

            // BASE ITEM TYPE ATTRIBUTES
            switch(eqSlot)
            {
                // HEAD
                case (int)EEqSlots.Head:
                    itemName = itemMaterial.Item1 + " Helmet";
                    itemStats.statsArr[(int)EStats.Armor].SetValue(1 * itemMaterial.Item2);
                    itemStats.statsArr[(int)EStats.HP].SetValue(2 * itemMaterial.Item2);
                    armBonus.Item1 *= 10f;
                    hpBonus.Item1 *= 5f;
                    conBonus.Item1 *= 3f;
                    break;

                // WEAPON
                case (int)EEqSlots.RHand:
                    itemName = itemMaterial.Item1 + " Sword";
                    itemStats.statsArr[(int)EStats.BaseDamage].SetValue(1 * itemMaterial.Item2);
                    strBonus.Item1 *= 15f;
                    dexBonus.Item1 *= 15f;
                    dmgBonus.Item1 *= 30f;
                    break;

                // SHIELD
                case (int)EEqSlots.LHand:
                    itemName = itemMaterial.Item1 + " Shield";
                    itemStats.statsArr[(int)EStats.Armor].SetValue(1 * itemMaterial.Item2);
                    armBonus.Item1 *= 25f;
                    strBonus.Item1 *= 5f;
                    dexBonus.Item1 *= 5f;
                    break;

                // BODY
                case (int)EEqSlots.Torso:
                    itemName = itemMaterial.Item1 + " Armor";
                    itemStats.statsArr[(int)EStats.Armor].SetValue(2 * itemMaterial.Item2);
                    itemStats.statsArr[(int)EStats.HP].SetValue(5 * itemMaterial.Item2);
                    armBonus.Item1 *= 50f;
                    hpBonus.Item1 *= 25f;
                    conBonus.Item1 *= 25f;
                    break;

                // FEET
                case (int)EEqSlots.Feet:
                    itemName = itemMaterial.Item1 + " Boots";
                    itemStats.statsArr[(int)EStats.Armor].SetValue(1 * itemMaterial.Item2);
                    itemStats.statsArr[(int)EStats.HP].SetValue(2 * itemMaterial.Item2);
                    armBonus.Item1 *= 15f;
                    dexBonus.Item1 *= 5f;
                    lucBonus.Item1 *= 3f;
                    break;

                // RIGHT FINGER
                case (int)EEqSlots.RFinger:
                    itemStats.statsArr[rng.Next(0, Enum.GetValues(typeof(EStats)).Length)].SetValue(1 * itemMaterial.Item2);
                    itemName = itemMaterial.Item1 + " Ring";
                    break;

                // LEFT FINGER
                case (int)EEqSlots.LFinger:
                    itemStats.statsArr[rng.Next(0, Enum.GetValues(typeof(EStats)).Length)].SetValue(1 * itemMaterial.Item2);
                    itemName = itemMaterial.Item1 + " Ring";
                    break;

                // DEFAULT TO RING
                default:
                    eqSlot = (int)EEqSlots.RFinger;
                    itemStats.statsArr[rng.Next(0, Enum.GetValues(typeof(EStats)).Length)].SetValue(1 * itemMaterial.Item2);
                    itemName = itemMaterial.Item1 + " Ring";
                    break;
            }

            // ADD BONUS OBJECTS TO LIST
            statBonusList.Add(strBonus);
            statBonusList.Add(dexBonus);
            statBonusList.Add(conBonus);
            statBonusList.Add(intBonus);
            statBonusList.Add(lucBonus);
            statBonusList.Add(hpBonus);
            statBonusList.Add(dmgBonus);
            statBonusList.Add(armBonus);

            // RANDOMIZE STAT BONUS COUNT
            do
            {
                bonusCounter++;
                addBonusChance *= bonusMod;
            } while (rng.NextDouble() < addBonusChance);

            // ADD BONUS STATS TO ITEM STATS OBJECT
            for (int i = 0; i < bonusCounter; i++)
            {
                var bonus = WeightedRandomFromList(statBonusList);
                itemStats.statsArr[(int)bonus.Item1].AddValue(bonus.Item2 * (float)(1 + (rng.NextDouble() * itemMaterial.Item2 * 0.5)));
            }

            // UPDATE ITEM NAME
            if ((bonusCounter - 2) > 0)
                itemName += $" +{bonusCounter - 2}";

            // CREATE & RETURN ITEM
            return new ItemEquippable(itemName, eqSlot, itemStats);
        }
    }
}
