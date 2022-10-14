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
        public WallTile(Vector2Int posXY, char wallSprite = '#', ConsoleColor wallColor = ConsoleColor.Gray) : base(posXY, wallSprite, wallColor) {}

        public bool ColliderIsActive => true;

        public void OnCollision()
        {
            throw new NotImplementedException();
        }
    }
}
