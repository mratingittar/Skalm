using Skalm.Map;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal class FloorTile : BaseTile
    {
        public FloorTile(Vector2Int posXY) : base(posXY, Globals.G_FLOOR, ConsoleColor.Gray) {}
    }
}
