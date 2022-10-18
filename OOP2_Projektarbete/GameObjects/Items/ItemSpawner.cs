using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class ItemSpawner : ISpawner<Item>
    {
        public Item Spawn(Vector2Int position, char sprite, ConsoleColor color)
        {
            Console.WriteLine("Item Spawned");
            return new Item(position, sprite, color);
        }
    }
}
