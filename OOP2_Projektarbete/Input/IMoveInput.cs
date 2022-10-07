using Skalm.Structs;

namespace Skalm.Input
{
    internal interface IMoveInput
    {
        public bool GetMoveInput(ConsoleKeyInfo key, out Vector2Int direction);
    }
}
