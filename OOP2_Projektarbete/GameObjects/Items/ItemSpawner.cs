using Skalm.GameObjects.Interfaces;
using Skalm.Structs;

namespace Skalm.GameObjects.Items
{
    internal class ItemSpawner : ISpawner<ItemPickup>, IScalable
    {
        public float ScalingMultiplier { get; set; }
        private float _scaledModifier => Math.Min(_baseModifier * (1 + 0.01f * ScalingMultiplier), 0.99f);
        private float _baseModifier;
        private ItemGen _itemGen;

        private char _itemSprite;
        private ConsoleColor _itemColor;

        // CONSTRUCTOR I
        public ItemSpawner(float baseModifier, char itemSprite, ConsoleColor itemColor, ItemGen itemGenerator)
        {
            _itemSprite = itemSprite;
            _itemColor = itemColor;
            _baseModifier = baseModifier;
            _itemGen = itemGenerator;
        }

        // SPAWN ITEM PICKUP
        public ItemPickup Spawn(Vector2Int position)
        {
            return new ItemPickup(position, _itemSprite, _itemColor, _itemGen.GetWeightedRandom(_scaledModifier));
        }
    }
}
