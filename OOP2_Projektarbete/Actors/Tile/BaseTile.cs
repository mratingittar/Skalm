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

        // PATHFINDING SHOULD MOVE TO RELEVANT TILE TYPES
        public BaseTile? Parent { get; set; }
        public int FCost { get => GCost + HCost; }
        public int GCost { get; set; }
        public int HCost { get; set; }

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
