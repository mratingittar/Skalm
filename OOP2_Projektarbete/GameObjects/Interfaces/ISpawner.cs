using Skalm.Structs;

namespace Skalm.GameObjects.Interfaces
{
    internal interface ISpawner<out T>
    {
        T Spawn(Vector2Int position);
    }
}