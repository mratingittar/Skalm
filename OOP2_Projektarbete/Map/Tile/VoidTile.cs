using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Map.Tile
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
