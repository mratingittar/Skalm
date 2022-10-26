using Skalm.GameObjects.Interfaces;
using Skalm.Structs;

namespace Skalm.Maps.Tiles
{
    internal class VoidTile : BaseTile, ICollider
    {
        public override char Sprite { get => ' '; }
        public bool ColliderIsActive { get => true; }
        public VoidTile(Vector2Int gridPosition) : base(gridPosition) { }

        public void OnCollision()
        {
        }
    }
}
