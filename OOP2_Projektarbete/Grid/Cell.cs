using Skalm.Structs;

namespace Skalm.Grid
{
    internal class Cell : IGridObject
    {
        public Vector2Int GridPosition { get; set; }
        public List<Vector2Int> ConsolePositions { get; set; }
        public IContentType Content { get; set; }

        public Cell(Vector2Int gridPosition, List<Vector2Int> consolePositions, IContentType content, bool print = true, bool fillCell = true)
        {
            GridPosition = gridPosition;
            ConsolePositions = consolePositions;
            Content = content;
            if (print)
                PrintContent(fillCell);
        }

        public void PrintContent(bool fillCell)
        {
            if (fillCell)
            {
                foreach (Vector2Int position in ConsolePositions)
                {
                    Console.SetCursorPosition(position.X, position.Y);
                    Console.Write(Content.Character);
                }
            }
            else
            {
                Console.SetCursorPosition(ConsolePositions.First().X, ConsolePositions.First().Y);
                Console.Write(Content.Character);
            }
        }
    }
}
