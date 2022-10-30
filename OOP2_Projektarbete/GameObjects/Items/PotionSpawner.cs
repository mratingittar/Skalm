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

        // SPAWN POTION
        public ItemPickup Spawn(Vector2Int position, float scalingMod = 1)
        {
            return new ItemPickup(position, _potionSprite, _potionColor, GetRandomPotion(_scaledModifier * scalingMod));
        }

        // GET RANDOM POTION
        private Potion GetRandomPotion(float scalingMod = 1)
        {
            Random rng = new Random();
            int healAmount = 5 + rng.Next(0,6);
            int bonusCounter = 0;
            float addHealChance = 0.8f * scalingMod;
            string potionName = "Potion of Healing";

            // RANDOMIZE HEAL AMOUNT
            do
            {
                bonusCounter++;
                healAmount += rng.Next((int)(1 * scalingMod), (int)(5 * scalingMod));
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
