using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.GameObjects.Items
{
    internal class ItemSpawner : ISpawner<ItemPickup>, IScalable
    {
        public float ScalingMultiplier { get; set; }
        private float _scaledModifier => Calculations.SpawningScalingEquation(_baseModifier, ScalingMultiplier);
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
        public ItemPickup Spawn(Vector2Int position, float scalingMod = 1)
        {
            return new ItemPickup(position, _itemSprite, _itemColor, _itemGen.GetWeightedRandom(_scaledModifier * scalingMod));
        }

        public ItemPickup Spawn(Vector2Int position, float experienceModifier)// <-- For monster drops, lacks implementation
        {
            return new ItemPickup(position, _itemSprite, _itemColor, _itemGen.GetWeightedRandom(_scaledModifier)); 
        }
    }
}
