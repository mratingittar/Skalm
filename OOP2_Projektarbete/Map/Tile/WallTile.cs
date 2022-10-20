using Skalm.GameObjects.Interfaces;
using Skalm.Map;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Map.Tile
{
    internal class WallTile : BaseTile, ICollider
    {
        // CONSTRUCTOR
        public WallTile(Vector2Int posXY, char wallSprite = '#', ConsoleColor wallColor = ConsoleColor.Gray) : base(posXY, wallSprite, wallColor) { }

        public bool ColliderIsActive { get => true; }

        public void OnCollision()
        {
            
        }

    }
}
