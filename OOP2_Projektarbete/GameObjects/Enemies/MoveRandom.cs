using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using Skalm.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Enemies
{
    internal class MoveRandom : IMoveBehaviour
    {
        public Vector2Int MoveDirection(Vector2Int currentPosition)
        {
            Random randomDir = new Random();
            return RandomDirection((Direction)randomDir.Next(4));
        }

        private Vector2Int RandomDirection(Direction dir) => dir switch
        {
            Direction.North => new Vector2Int(0, -1),
            Direction.East => new Vector2Int(1, 0),
            Direction.South => new Vector2Int(0, 1),
            Direction.West => new Vector2Int(-1, 0),
            _ => throw new InvalidOperationException("Not a direction"),
        };
    }
}
