using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Spawning
{
    internal interface ISpawner<out T>
    {
        T Spawn(Vector2Int position, char sprite, ConsoleColor color);
    }
}
