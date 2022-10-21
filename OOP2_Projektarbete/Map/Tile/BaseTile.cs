using Skalm.GameObjects.Interfaces;
using Skalm.Map;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Map.Tile
{
    internal abstract class BaseTile : IGridObject
    {
        protected char _sprite;
        protected ConsoleColor _color;

        public Vector2Int GridPosition { get; protected set; }
        public virtual char Sprite { get => _sprite; }
        public virtual bool ShowSingleSprite { get; protected set; }
        public virtual ConsoleColor Color { get => _color; }
        public virtual string Label { get; protected set; }

        // PATHFINDING PROPERTIES
        public BaseTile Parent { get; set; }
        public int FCost { get => GCost + HCost; }
        public int GCost { get; set; }
        public int HCost { get; set; }


        public BaseTile(Vector2Int gridPosition, char sprite = ' ', ConsoleColor color = ConsoleColor.White)
        {
            GridPosition = gridPosition;
            _sprite = sprite;
            _color = color;
            Label = GetType().Name;
            Parent = this;
        }
    }
}
