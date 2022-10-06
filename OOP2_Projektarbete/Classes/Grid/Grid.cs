using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes.Grid
{
    internal class Grid<T> where T : IGridObject
    {
        public readonly int gridWidth;
        public readonly int gridHeight;
        private int cellWidth;
        private int cellHeight;
        private Vector2Int origin;
        public T[,] GridArray;
        public readonly int consoleWidth;
        public readonly int consoleHeight;

        public Grid(int width, int height, int cellWidth, int cellHeight, Vector2Int origin, Func<Vector2Int, List<Vector2Int>, T> createGridObject)
        {
            gridWidth = width;
            gridHeight = height;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.origin = origin;
            consoleWidth = width * cellWidth;
            consoleHeight = height * cellHeight;

            GridArray = new T[width, height];

            for (int x = 0; x < GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < GridArray.GetLength(1); y++)
                {
                    GridArray[x, y] = createGridObject(new Vector2Int(x,y), GetConsolePositions(new Vector2Int(x,y)));
                }
            }
        }


        public void PrintPartialGrid()
        {
            try
            {
                if (Console.BufferHeight < gridHeight*cellHeight+origin.Y && OperatingSystem.IsWindows())
                    Console.BufferHeight = gridHeight*cellHeight+origin.Y;

                if (Console.BufferWidth < gridWidth*cellWidth+origin.X && OperatingSystem.IsWindows())
                    Console.BufferWidth = gridWidth*cellWidth+origin.X;

                for (int x = 0; x < GridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < GridArray.GetLength(1); y++)
                    {
                        Vector2Int pos = GridArray[x, y].ConsolePositions[0];

                        Console.SetCursorPosition(pos.X, pos.Y);
                        Console.Write(GridArray[x, y].CharacterRepresentation);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void PrintFullGrid()
        {
            for (int x = 0; x < GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < GridArray.GetLength(1); y++)
                {
                    foreach (Vector2Int position in GridArray[x,y].ConsolePositions)
                    {
                        Console.SetCursorPosition(position.X, position.Y);
                        Console.Write(GridArray[x, y].CharacterRepresentation);
                    }
                }
            }
        }

        private List<Vector2Int> GetConsolePositions(Vector2Int gridPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            for (int w = 0; w < cellWidth; w++)
            {
                for (int h = 0; h < cellHeight; h++)
                {
                    positions.Add(new Vector2Int(gridPosition.X * cellWidth + origin.X+w, gridPosition.Y * cellHeight + origin.Y+h));
                }
            }
            return positions;
        }

        private Vector2Int GetConsolePosition(Vector2Int gridPosition)
        {
            return new Vector2Int(gridPosition.X * cellWidth + origin.X, gridPosition.Y * cellHeight + origin.Y);
        }

        private Vector2Int GetGridPosition(Vector2Int consolePosition)
        {
            int x = (consolePosition.X - origin.X) / cellWidth;
            int y = (consolePosition.Y - origin.Y) / cellHeight;
            return new Vector2Int(x,y);
        }

        public T? GetGridObject(Vector2Int gridPosition)
        {
            if (gridPosition.X >= 0 && gridPosition.Y >= 0 && gridPosition.X < gridWidth && gridPosition.Y < gridHeight)
                return GridArray[gridPosition.X, gridPosition.Y];
            else
                return default;
        }


        public void SetGridObject(Vector2Int gridPosition, T obj)
        {
            if (gridPosition.X >= 0 && gridPosition.Y >= 0 && gridPosition.X < gridWidth && gridPosition.Y < gridHeight)
                GridArray[gridPosition.X, gridPosition.Y] = obj;
        }
    }
}
