using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal class VoidTile : BaseTile, ICollider
    {
        public override char Sprite { get => ' '; }

        public bool ColliderIsActive => true;

        public VoidTile(Vector2Int gridPosition) : base(gridPosition) {}

        public void OnCollision()
        {
            throw new NotImplementedException();
        }
    }
}
