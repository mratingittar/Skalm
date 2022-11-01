using Skalm.GameObjects.Interfaces;
using Skalm.Structs;


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
