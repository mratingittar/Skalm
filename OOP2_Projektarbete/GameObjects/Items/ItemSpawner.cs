using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class ItemSpawner : ISpawner<ItemPickup>
    {
        // SPAWN ITEM PICKUP
        public ItemPickup Spawn(Vector2Int position, char sprite, ConsoleColor color)
        {
            Console.WriteLine("Item Spawned");

            // TO DO: SPAWN ACTUAL ITEM
            return new ItemPickup(position, sprite, color, new Item("New Item"));
        }
    }
}
