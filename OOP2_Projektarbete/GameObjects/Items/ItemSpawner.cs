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

        public ItemSpawner(float baseModifier, ItemGen itemGenerator)
        {
            _baseModifier = baseModifier;
            _itemGen = itemGenerator;
        }

        public ItemPickup Spawn(Vector2Int position, char sprite, ConsoleColor color)
        {
            return new ItemPickup(position, sprite, color, _itemGen.GetWeightedRandom(_scaledModifier));
        }


    }
}
