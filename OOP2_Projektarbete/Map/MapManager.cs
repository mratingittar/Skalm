
using Skalm.Grid;
using Skalm.Structs;

namespace Skalm.Map
{
    internal class MapManager
    {
        private Vector2Int mapOrigin;
        private int mapWidth;
        private int mapHeight;
        public Grid<Cell> mapGrid;
        //public int[,] mapArr { get; private set; }

        // CONSTRUCTOR I
        public MapManager(int width, int height, Vector2Int origin)
        {
            mapWidth = width;
            mapHeight = height;
            mapOrigin = origin;
            mapGrid = new Grid<Cell>(width, height, 2, 1, origin, (gridPosition, consolePositions) => new Cell(gridPosition, consolePositions, new CellEmpty()));


            //mapArr = new int[mapWidth, mapHeight];
            //InitMap();
        }

        // METHOD INITIALIZE MAP
        public void InitMap(int[,] mapArr)
        {
            for (int j = 4; j < mapArr.GetLength(1) - 4; j++)
            {
                for (int i = 4; i < mapArr.GetLength(0) - 4; i++)
                {
                    mapArr[i, j] = (int)MapTiles.Floor;
                }
            }

            FindWalls(mapArr);
        }

        // METHOD CELL VON NEUMAN
        private int cell4W(int[,] mapArr, int x, int y, int value)
        {
            int output = 0;
            if (mapArr[x, y - 1] == value) output++;
            if (mapArr[x, y + 1] == value) output++;
            if (mapArr[x - 1, y] == value) output++;
            if (mapArr[x + 1, y] == value) output++;
            return output;
        }

        // FIND WALLS IN ARRAY
        private void FindWalls(int[,] mapArr)
        {
            for (int j = 0; j < mapArr.GetLength(1); j++)
            {
                for (int i = 0; i < mapArr.GetLength(0); i++)
                {
                    if (mapArr[i, j] == (int)MapTiles.Floor)
                        if (cell4W(mapArr, i, j, (int)MapTiles.Void) > 0) mapArr[i, j] = (int)MapTiles.Wall;
                }
            }
        }
    }

    public enum MapTiles
    {
        Void,
        Wall,
        Floor,
        Door
    }
}
