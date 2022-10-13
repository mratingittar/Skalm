using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Spawning
{
    internal class MonsterSpawner : ISpawner<Monster>
    {
        public Monster Spawn()
        {
            Console.WriteLine("Monster Spawned");
            return new Monster();
        }
    }
}
