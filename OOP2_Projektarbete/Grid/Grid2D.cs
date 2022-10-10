using Skalm.Structs;

namespace Skalm.Grid
{
    internal class Grid2D<T>
    {
        public readonly int gridWidth;
        public readonly int gridHeight;
        public readonly int cellWidth;
        public readonly int cellHeight;
        public readonly Vector2Int origin;
        public T[,] GridArray { get; private set; }

        public Grid2D(int width, int height, int cellWidth, int cellHeight, Vector2Int origin, Func<int, int, List<Vector2Int>, T> createGridObject)
        {
            gridWidth = width;
            gridHeight = height;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.origin = origin;

            GridArray = new T[width, height];

            for (int x = 0; x < GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < GridArray.GetLength(1); y++)
                {
                    GridArray[x, y] = createGridObject(x, y, GetPlanePositions(x, y));
                }
            }
        }

        private List<Vector2Int> GetPlanePositions(int gridX, int gridY)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            for (int w = 0; w < cellWidth; w++)
            {
                for (int h = 0; h < cellHeight; h++)
                {
                    positions.Add(new Vector2Int(gridX * cellWidth + origin.X + w, gridY * cellHeight + origin.Y + h));
                }
            }
            return positions;
        }

  
        public Vector2Int GetPlanePosition(int gridX, int gridY)
        {
            return new Vector2Int(gridX * cellWidth + origin.X, gridY * cellHeight + origin.Y);
        }


        public (int, int) GetGridPosition(Vector2Int planePosition)
        {
            int x = (planePosition.X - origin.X) / cellWidth;
            int y = (planePosition.Y - origin.Y) / cellHeight;
            return (x, y);
        }

        public T? GetGridObject(int gridX, int gridY)
        {
            if (gridX >= 0 && gridY >= 0 && gridX < gridWidth && gridY < gridHeight)
                return GridArray[gridX, gridY];
            else
                return default;
        }

        public void SetGridObject(int gridX, int gridY, T obj)
        {
            if (gridX >= 0 && gridY >= 0 && gridX < gridWidth && gridY < gridHeight)
                GridArray[gridX, gridY] = obj;
        }
    }
}
