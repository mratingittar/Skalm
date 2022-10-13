using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal class VoidTile : BaseTile
    {
        public VoidTile(Vector2Int posXY, char sprite = ' ', ConsoleColor color = ConsoleColor.White) : base(posXY) {}
    }
}
