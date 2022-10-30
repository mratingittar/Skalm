using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.GameObjects.Items
{
    internal class PotionSpawner : ISpawner<ItemPickup>, IScalable
    {
        public float ScalingMultiplier { get; set; }
        private float _scaledModifier => Calculations.SpawningScalingEquation(_baseModifier, ScalingMultiplier);
        private float _baseModifier;
        private char _potionSprite;
        private ConsoleColor _potionColor;

        // CONSTRUCTOR I
        public PotionSpawner(float baseModifier, char potionSprite, ConsoleColor potionColor)
        {
            _baseModifier = baseModifier;
            _potionSprite = potionSprite;
            _potionColor = potionColor;
        }

        // SPAWN POTION
        public ItemPickup Spawn(Vector2Int position)
        {
            return new ItemPickup(position, _potionSprite, _potionColor, GetRandomPotion());
        }

        // GET RANDOM POTION
        private Potion GetRandomPotion()
        {
            Random rng = new Random();
            int healAmount = 5;
            int bonusCounter = 0;
            float addHealChance = 0.75f;
            string potionName = "Potion of Healing";

            // RANDOMIZE HEAL AMOUNT
            do
            {
                bonusCounter++;
                healAmount += rng.Next(1, 4);
                addHealChance *= _scaledModifier;
            } while (rng.NextDouble() < addHealChance);

            // UPDATE ITEM NAME
            if ((bonusCounter - 2) > 0)
                potionName += $" +{bonusCounter - 2}";

            // CREATE & RETURN POTION
            return new Potion(potionName, healAmount);
        }
    }
}
