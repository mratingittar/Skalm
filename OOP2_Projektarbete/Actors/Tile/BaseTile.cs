using Skalm.Map;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal abstract class BaseTile : IGridObject
    {
        public Vector2Int GridPosition { get; protected set; }
        public virtual char Sprite { get; protected set; }
        public ConsoleColor Color { get; protected set; }

        public BaseTile(Vector2Int gridPosition, char sprite = ' ', ConsoleColor color = ConsoleColor.White)
        {
            GridPosition = gridPosition;
            Sprite = sprite;
            Color = color;
        }
        public virtual char GetSprite()
        {
                return Sprite;
        }

        public virtual ConsoleColor GetColor()
        {
                return Color;
        }
    }
}
