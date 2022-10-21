using Skalm.Structs;

namespace Skalm.GameObjects.Interfaces
{
    internal interface IGridObject
    {
        Vector2Int GridPosition { get; }
        char Sprite { get; }
        bool ShowSingleSprite { get; }
        ConsoleColor Color { get; }
        string Label { get; }
    }
}
