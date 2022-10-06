using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes
{
    internal class Cell
    {
        public Cell(Vector2Int consolePosition, int x, int y)
        {
            ConsolePosition = consolePosition;
            GridPosition = new Vector2Int(x,y);
        }

        public Vector2Int ConsolePosition { get; private set; }
        public Vector2Int GridPosition { get; private set; }
    }
}
