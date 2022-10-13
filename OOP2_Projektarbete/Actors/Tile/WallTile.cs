using Skalm.Map;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal class WallTile : BaseTile, ICollidable
    {
        // CONSTRUCTOR
        public WallTile(Vector2Int posXY) : base(posXY, Globals.G_WALL, ConsoleColor.Gray) {}

        public void OnCollision()
        {

        }
    }
}
