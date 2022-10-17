using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors
{
    internal abstract class Actor : IGridObject, IMoveable, ICollider, IGameObject
    {
        public Vector2Int GridPosition { get; protected set; }

        public char Sprite { get; protected set; }

        public ConsoleColor Color { get; protected set; }

        public bool ColliderIsActive { get => true; }
        public static event Action<Actor, Vector2Int, Vector2Int>? OnPositionChanged;
        public static event Func<Vector2Int, bool>? OnMoveRequested;

        public Actor(Vector2Int gridPosition, char sprite, ConsoleColor color)
        {
            GridPosition = gridPosition;
            Sprite = sprite;
            Color = color;
        }

        public virtual void Move(Vector2Int direction)
        {
            direction.Normalize();
            Vector2Int newPosition = GridPosition.Add(direction);
            var collision = OnMoveRequested?.Invoke(newPosition);
            if (collision is not null)
                if (!collision.Value)
                    ExecuteMove(newPosition, GridPosition);
        }

        private void ExecuteMove(Vector2Int newPosition, Vector2Int oldPosition)
        {
            GridPosition = newPosition;
            OnPositionChanged?.Invoke(this, newPosition, oldPosition);
        }

        public void OnCollision()
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateMain()
        {
            
        }
    }
}
