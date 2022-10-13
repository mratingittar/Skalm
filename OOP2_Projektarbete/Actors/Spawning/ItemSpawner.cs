using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Spawning
{
    internal class ItemSpawner : ISpawner<Item>
    {
        public Item Spawn()
        {
            Console.WriteLine("Item Spawned");
            return new Item();
        }
    }
}
