using Skalm.GameObjects.Interfaces;
using Skalm.Structs;

namespace Skalm.Maps.Tiles
{
    internal class WallTile : BaseTile, ICollider
    {
        public override string Label { get => "wall"; }

        // CONSTRUCTOR
        public WallTile(Vector2Int posXY, char wallSprite = '#', ConsoleColor wallColor = ConsoleColor.Gray) : base(posXY, wallSprite, wallColor) { }

        public bool ColliderIsActive { get => true; }

        public void OnCollision()
        {

        }

    }
}
