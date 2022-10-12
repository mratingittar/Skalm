using Skalm.Structs;

namespace Skalm.Actors.StateMachine
{
    internal class Tile
    {
        public readonly Vector2Int position;

        public Tile(Vector2Int position)
        {
            this.position = position;
        }
    }
}
