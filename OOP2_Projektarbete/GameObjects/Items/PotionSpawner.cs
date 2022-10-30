using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.GameObjects.Items
{
    internal class PotionSpawner : ISpawner<ItemPickup>, IScalable
    {
        public float ScalingMultiplier { get; set; }
        private float _scaledModifier => Math.Min(_baseModifier * (1 + 0.01f * ScalingMultiplier), 0.99f);
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

        // SPAWN POTION PICKUP
        public ItemPickup Spawn(Vector2Int position, float scalingMod = 1) => new ItemPickup(position, _potionSprite, _potionColor, GetRandomPotion(scalingMod));

        // GET POTION ITEM
        public Potion GetItemPotion(float scalingMod = 1) => GetRandomPotion(scalingMod);

        // GET RANDOM POTION
        private Potion GetRandomPotion(float scalingMod = 1)
        {
            Random rng = new Random();
            int healAmount = 5 + rng.Next(0,6);
            int bonusCounter = 0;
            float addHealChance = 0.8f * scalingMod;
            string potionName = "Potion of Healing";
            int minRng, maxRng;

            // RANDOMIZE HEAL AMOUNT
            do
            {
                bonusCounter++;
                minRng = Math.Max(1, (int)(1 * scalingMod));
                maxRng = Math.Max(1, (int)(5 * scalingMod));
                healAmount += rng.Next(minRng, maxRng);
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
