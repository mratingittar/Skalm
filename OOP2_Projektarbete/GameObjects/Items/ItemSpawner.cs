using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class ItemSpawner : ISpawner<ItemPickup>, IScalable
    {
        public float ScalingMultiplier { get; set; }
        private ItemGen _itemGen;

        public ItemSpawner(ItemGen itemGenerator)
        {
            ScalingMultiplier = 1;
            _itemGen = itemGenerator;
        }

        public ItemPickup Spawn(Vector2Int position, char sprite, ConsoleColor color)
        {
            return new ItemPickup(position, sprite, color, _itemGen.GetWeightedRandom());
        }


    }
}
