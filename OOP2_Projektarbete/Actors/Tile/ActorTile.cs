using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal class ActorTile : BaseTile
    {
        public ActorTile(Vector2Int posXY, char sprite = ' ', ConsoleColor color = ConsoleColor.White) : base(posXY, sprite, color) {}
    }
}
