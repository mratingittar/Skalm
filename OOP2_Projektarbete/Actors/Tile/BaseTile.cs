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
        public List<Actor> actorsAtPosition;

        public BaseTile(Vector2Int gridPosition, char sprite = ' ', ConsoleColor color = ConsoleColor.White)
        {
            this.GridPosition = gridPosition;
            this.Sprite = sprite;
            this.Color = color;

            actorsAtPosition = new List<Actor>();
        }
        public char GetSprite()
        {
            if (actorsAtPosition.Count == 0)
                return Sprite;
            else
                return actorsAtPosition.First().Sprite;
        }

        public ConsoleColor GetColor()
        {
            if (actorsAtPosition.Count == 0)
                return Color;
            else
                return actorsAtPosition.First().Color;
        }
    }
}
