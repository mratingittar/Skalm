using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Spawning
{
    internal class EnemySpawner : ISpawner<Enemy>
    {
        public Enemy Spawn(Vector2Int gridPosition, char sprite, ConsoleColor color)
        {
            Console.WriteLine("Enemy Spawned");
            return new Enemy(gridPosition, sprite, color);
        }
    }
}
