using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes.Grid
{
    internal class Grid<T>
    {
        private int width;
        private int height;
        private int cellWidth;
        private int cellHeight;
        private Vector2Int origin;
        private T[,] gridArray;

        public Grid(int width, int height, int cellWidth, int cellHeight, Vector2Int origin, Func<Vector2Int, int, int, T> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.origin = origin;

            gridArray = new T[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createGridObject(GetConsolePosition(x, y), x, y);
                }
            }
        }

        public void PrintGrid(char character)
        {
            try
            {
                if (Console.BufferHeight < height*cellHeight+origin.Y)
                    Console.BufferHeight = height*cellHeight+origin.Y;

                if (Console.BufferWidth < width*cellWidth+origin.X)
                    Console.BufferWidth = width*cellWidth+origin.X;

                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        Vector2Int pos = GetConsolePosition(x, y);

                        Console.SetCursorPosition(pos.X, pos.Y);
                        Console.Write(character);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private Vector2Int GetConsolePosition(int x, int y)
        {
            return new Vector2Int(x * cellWidth + origin.X, y * cellHeight + origin.Y);
        }

        private (int x, int y) GetGridPosition(Vector2Int consolePosition)
        {
            int x = (consolePosition.X - origin.X) / cellWidth;
            int y = (consolePosition.Y - origin.Y) / cellHeight;
            return (x, y);
        }

        public T? GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                return gridArray[x, y];
            else
                return default;
        }

        public T? GetGridObject(Vector2Int position)
        {
            (int x, int y) = GetGridPosition(position);
            return GetGridObject(x, y);
        }

        public void SetGridObject(int x, int y, T obj)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                gridArray[x, y] = obj;
        }

        public void SetGridObject(Vector2Int position, T obj)
        {
            (int x, int y) = GetGridPosition(position);
            SetGridObject(x, y, obj);
        }
    }
}
