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
        public char sprite { get; protected set; }
        public ConsoleColor color { get; protected set; }
        public int drawDepth { get; protected set; }

        public Tile(Vector2Int posXY, char sprite = ' ', ConsoleColor color = ConsoleColor.White, int drawDepth = 0)
        {
            this.posXY = posXY;
            this.sprite = sprite;
            this.color = color;
            this.drawDepth = drawDepth;
        }
    }
}
