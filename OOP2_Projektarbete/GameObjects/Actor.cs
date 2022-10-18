using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects
{
    internal abstract class Actor : GameObject, ICollider
    {
        public bool ColliderIsActive { get { return true; } set { } }

        public static event Action<Actor, Vector2Int, Vector2Int>? OnPositionChanged;
        public static event Func<Vector2Int, bool>? OnMoveRequested;

        public Actor(Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
        }

        // METHOD MOVE
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

        // METHOD COLLISION
        public void OnCollision()
        {
            throw new NotImplementedException();
        }

        // METHOD UPDATE
        public virtual void UpdateMain()
        {

        }
    }
}
