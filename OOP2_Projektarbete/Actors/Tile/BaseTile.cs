using Skalm.Map;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal abstract class BaseTile
    {
        public Vector2Int posXY { get; protected set; }
        public char sprite { get; protected set; }
        public ConsoleColor color { get; protected set; }
        public int drawDepth { get; protected set; }
        public MapManager.MapTiles tileType { get; protected set; }

        public BaseTile(Vector2Int posXY, char sprite = ' ', ConsoleColor color = ConsoleColor.White, MapManager.MapTiles tileType = MapManager.MapTiles.Floor,  int drawDepth = 0)
        {
            this.posXY = posXY;
            this.sprite = sprite;
            this.color = color;
            this.drawDepth = drawDepth;
            this.tileType = tileType;
        }
    }
}
