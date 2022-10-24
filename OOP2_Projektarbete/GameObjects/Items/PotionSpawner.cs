using Skalm.GameObjects.Interfaces;
using Skalm.Structs;

namespace Skalm.GameObjects.Items
{
    internal class PotionSpawner : ISpawner<ItemPickup>, IScalable
    {
        public float ScalingMultiplier { get; set; }
        private float _scaledModifier => Math.Min(_baseModifier * (1 + 0.01f * ScalingMultiplier), 0.99f);
        private float _baseModifier;
        public PotionSpawner(float baseModifier)
        {
            ScalingMultiplier = 1;
            _baseModifier = baseModifier;
        }
        public ItemPickup Spawn(Vector2Int position, char sprite, ConsoleColor color)
        {
            return new ItemPickup(position, sprite, color, GetRandomPotion());
        }

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
