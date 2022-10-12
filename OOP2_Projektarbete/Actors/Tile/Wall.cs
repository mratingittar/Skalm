using Skalm.Map;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal class Wall : BaseTile, ICollidable
    {
        // CONSTRUCTOR
        public Wall(Vector2Int posXY) : base(posXY, Globals.G_WALL, ConsoleColor.Gray, MapManager.MapTiles.Wall) {}

        public void OnCollision()
        {

        }
    }
}
