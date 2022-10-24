using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class KeySpawner : ISpawner<ItemPickup>
    {
        public ItemPickup Spawn(Vector2Int position, char sprite, ConsoleColor color)
        {
            return new ItemPickup(position, sprite, color, new Key());
        }
    }
}
