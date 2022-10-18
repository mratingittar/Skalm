using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Enemies
{
    internal class MoveIdle : IMoveBehaviour
    {
        public Vector2Int MoveDirection(Vector2Int currentPosition)
        {
            return Vector2Int.Zero;
        }
    }
}
