using Skalm.Structs;

namespace Skalm.Grid
{
    internal class Cell
    {
        public readonly Vector2Int gridPosition;
        public readonly List<Vector2Int> ConsolePositions;
        public IHUD PartOfHUD { get; set; }

        public Cell(Vector2Int gridPosition, List<Vector2Int> consolePositions, IHUD partOfHUD)
        {
            this.gridPosition = gridPosition;
            ConsolePositions = consolePositions;
            PartOfHUD = partOfHUD;
        }
    }
}
