using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects
{
    internal class GameObject : IGridObject
    {
        // FIELDS
        protected char _sprite;
        protected ConsoleColor _color;

        // PROPERTIES
        public Vector2Int GridPosition { get; protected set; }
        public char Sprite { get => _sprite; }
        public ConsoleColor Color { get => _color; }
        public virtual string Label { get; protected set; }

        // CONSTRUCTOR I
        public GameObject(Vector2Int gridPosition, char sprite, ConsoleColor color)
        {
            GridPosition = gridPosition;
            _sprite = sprite;
            _color = color;
            Label = GetType().Name;
        }
    }
}
