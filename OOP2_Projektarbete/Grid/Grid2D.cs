using Skalm.Structs;
using System;

namespace Skalm.Grid
{
    internal class Grid2D<T>
    {
        public readonly int gridWidth;
        public readonly int gridHeight;
        public readonly int cellWidth;
        public readonly int cellHeight;
        public readonly Vector2Int origin;
        private T[,] gridArray;

        public Grid2D(int width, int height, int cellWidth, int cellHeight, Vector2Int origin, Func<int, int, T> createGridObject)
        {
            gridWidth = width;
            gridHeight = height;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.origin = origin;

            gridArray = new T[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createGridObject(x, y);
                }
            }
        }

        public List<Vector2Int> GetPlanePositions(int gridX, int gridY)
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

        public List<Vector2Int> GetPlanePositions(Vector2Int gridPosition)
        {
            return GetPlanePositions(gridPosition.X, gridPosition.Y);
        }

  
        public Vector2Int GetPlanePosition(int gridX, int gridY)
        {
            return new Vector2Int(gridX * cellWidth + origin.X, gridY * cellHeight + origin.Y);
        }

        public Vector2Int GetPlanePosition(Vector2Int gridPosition)
        {
            return GetPlanePosition(gridPosition.X, gridPosition.Y);
        }


        public (int, int) GetGridPosition(Vector2Int planePosition)
        {
            int x = (planePosition.X - origin.X) / cellWidth;
            int y = (planePosition.Y - origin.Y) / cellHeight;
            return (x, y);
        }

        public T? GetGridObject(Vector2Int gridPosition)
        {
            return GetGridObject(gridPosition.X, gridPosition.Y);
        }

        public T? GetGridObject(int gridX, int gridY)
        {
            if (gridX >= 0 && gridY >= 0 && gridX < gridWidth && gridY < gridHeight)
                return gridArray[gridX, gridY];
            else
                return default;
        }


        public bool TryGetGridObject(Vector2Int gridPosition, out T obj)
        {
            return TryGetGridObject(gridPosition.X, gridPosition.Y, out obj);
        }

        public bool TryGetGridObject(int gridX, int gridY, out T obj)
        {
            if (gridX >= 0 && gridY >= 0 && gridX < gridWidth && gridY < gridHeight)
            {
                obj = gridArray[gridX, gridY];
                return true;
            }
            else
            { 
                obj = gridArray[0,0];
                return false;
            }
        }

        public void SetGridObject(int gridX, int gridY, T obj)
        {
            if (gridX >= 0 && gridY >= 0 && gridX < gridWidth && gridY < gridHeight)
                gridArray[gridX, gridY] = obj;
        }
    }
}
