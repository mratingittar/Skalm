using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal abstract class Wall : Tile, ICollidable
    {
        // CONSTRUCTOR
        protected Wall(Vector2Int posXY) : base(posXY, Globals.G_WALL, ConsoleColor.Gray) {}

        public void OnCollision()
        {

        }
    }
}
