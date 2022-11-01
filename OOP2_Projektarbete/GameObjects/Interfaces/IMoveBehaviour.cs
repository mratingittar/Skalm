using Skalm.Structs;


namespace Skalm.GameObjects.Interfaces
{
    internal interface IMoveBehaviour
    {
        Vector2Int MoveDirection(Vector2Int currentPosition);
    }
}
