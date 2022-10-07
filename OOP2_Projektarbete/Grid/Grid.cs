using Skalm.Display;
using Skalm.Structs;

namespace Skalm.Grid
{
    internal class Grid<T> where T : IGridObject
    {
        // TODO: SEPARATE GENERIC GRID LOGIC FROM CONSOLE GRID LOGIC

        public readonly int gridWidth;
        public readonly int gridHeight;
        private int cellWidth;
        private int cellHeight;
        private Vector2Int origin;
        private T[,] gridArray;
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

            gridArray = new T[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createGridObject(new Vector2Int(x, y), GetConsolePositions(new Vector2Int(x, y)));
                }
            }
        }

        /// <summary>
        /// Assigns content type to all cells within specified bounds.
        /// </summary>
        /// <param name="area">Area to be defined.</param>
        /// <param name="content">Content type to assign.</param>
        public void DefineContentOfGridArea(Bounds area, IContentType content)
        {
            for (int x = area.StartXY.X; x < area.EndXY.X; x++)
            {
                for (int y = area.StartXY.Y; y < area.EndXY.Y; y++)
                {
                    gridArray[x, y].Content = content;
                }
            }
        }

        /// <summary>
        /// Prints content character to every cell in grid that matches content type.
        /// </summary>
        /// <param name="content">Specifies which type of content to match.</param>
        public void PrintGridContent(IContentType content) // MAYBE FIND WAY TO ONLY PASS TYPE AS PARAMETER
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    if (gridArray[x, y].Content.GetType() == content.GetType())
                        PrintCell(gridArray[x, y].ConsolePositions, gridArray[x, y].Content.Character);
                }
            }
        }

        /// <summary>
        /// Prints content character to every cell in grid.
        /// </summary>
        /// <param name="fullGrid">Covers every console cell if true.</param>
        public void PrintGridContent(bool fullGrid = true)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    if (fullGrid)
                    {
                        PrintCell(gridArray[x, y].ConsolePositions, gridArray[x, y].Content.Character);
                    }
                    else
                    {
                        Vector2Int pos = gridArray[x, y].ConsolePositions.First();
                        DisplayManager.PrintAtPosition(pos.X, pos.Y, gridArray[x, y].Content.Character);
                    }
                }
            }
        }

        /// <summary>
        /// Prints content character to all console cells in list.
        /// </summary>
        /// <param name="consolePositions"></param>
        /// <param name="character"></param>
        private void PrintCell(List<Vector2Int> consolePositions, char character)
        {
            foreach (Vector2Int position in consolePositions)
            {
                DisplayManager.PrintAtPosition(position.X, position.Y, character);
            }
        }

        private List<Vector2Int> GetConsolePositions(Vector2Int gridPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            for (int w = 0; w < cellWidth; w++)
            {
                for (int h = 0; h < cellHeight; h++)
                {
                    positions.Add(new Vector2Int(gridPosition.X * cellWidth + origin.X + w, gridPosition.Y * cellHeight + origin.Y + h));
                }
            }
            return positions;
        }

        public Vector2Int GetConsolePosition(Vector2Int gridPosition)
        {
            return new Vector2Int(gridPosition.X * cellWidth + origin.X, gridPosition.Y * cellHeight + origin.Y);
        }

        public Vector2Int GetConsolePosition(int x, int y)
        {
            return GetConsolePosition(new Vector2Int(x, y));
        }

        public Vector2Int GetGridPosition(Vector2Int consolePosition)
        {
            int x = (consolePosition.X - origin.X) / cellWidth;
            int y = (consolePosition.Y - origin.Y) / cellHeight;
            return new Vector2Int(x, y);
        }

        public T? GetGridObject(Vector2Int gridPosition)
        {
            if (gridPosition.X >= 0 && gridPosition.Y >= 0 && gridPosition.X < gridWidth && gridPosition.Y < gridHeight)
                return gridArray[gridPosition.X, gridPosition.Y];
            else
                return default;
        }

        public void SetGridObject(int x, int y, T obj)
        {
            SetGridObject(new Vector2Int(x, y), obj);
        }

        public void SetGridObject(Vector2Int gridPosition, T obj)
        {
            if (gridPosition.X >= 0 && gridPosition.Y >= 0 && gridPosition.X < gridWidth && gridPosition.Y < gridHeight)
                gridArray[gridPosition.X, gridPosition.Y] = obj;
        }
    }
}
