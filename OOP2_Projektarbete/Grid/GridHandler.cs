using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Grid
{
    internal class GridHandler<T>
    {
        private Grid2D<T> grid;

        public GridHandler(Grid2D<T> grid)
        {
            this.grid = grid;
        }

        public Vector2Int GetSingleConsolePositionFromGridPosition(int gridX, int gridY)
        {
            return grid.GetPlanePosition(gridX, gridY);
        }
        public List<Vector2Int> GetAllConsolePositionsFromGridPosition(int gridX, int gridY)
        {
            return grid.GetPlanePositions(gridX, gridY);
        }
        public bool TryGetGridObject(int gridX, int gridY, out T obj)
        {
            if (grid.GetGridObject(gridX, gridY) is not null)
            {
                obj = grid.GridArray[gridX, gridY];
                return true;
            }
            else
            {
                obj = grid.GridArray[0,0];
                return false;
            }
        }
    }
}
