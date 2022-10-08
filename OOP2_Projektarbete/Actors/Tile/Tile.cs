using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal abstract class Tile
    {
        public Vector2Int posXY { get; protected set; }
        protected char sprite;

        public Tile(Vector2Int posXY, char sprite)
        {
            this.posXY = posXY;
            this.sprite = sprite;
        } 
    }
}
