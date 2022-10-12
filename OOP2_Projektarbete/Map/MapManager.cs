
using Skalm.Actors.Tile;
using Skalm.Grid;
using Skalm.Structs;

namespace Skalm.Map
{
    internal class MapManager
    {
        private Vector2Int mapOrigin;
        private int mapWidth;
        private int mapHeight;
        public Tile[,] mapGrid;
        //public Grid<Cell> mapGrid;
        //public int[,] mapArr { get; private set; }

        // CONSTRUCTOR I
        public MapManager(int width, int height, Vector2Int origin)
        {
            mapWidth = width;
            mapHeight = height;
            mapOrigin = origin;
            //mapGrid = new Grid<Cell>(width, height, 2, 1, origin, (gridPosition, consolePositions) => new Cell(gridPosition, consolePositions, new CellEmpty()));
            mapGrid = new Tile[mapWidth, mapHeight];

            //mapArr = new int[mapWidth, mapHeight];
            InitMap(mapGrid);
        }

        // METHOD INITIALIZE MAP
        public void InitMap(Tile[,] mapArr)
        {
            for (int j = 4; j < mapArr.GetLength(1) - 4; j++)
            {
                for (int i = 4; i < mapArr.GetLength(0) - 4; i++)
                {
                    //mapArr[i, j] = (int)MapTiles.Floor;
                    mapArr[i, j] = new Floor(new Vector2Int(i, j));
                }
            }

            FindWalls(mapArr);
        }

        // FIND WALLS IN ARRAY
        private void FindWalls(Tile[,] mapArr)
        {
            for (int j = 0; j < mapArr.GetLength(1); j++)
            {
                for (int i = 0; i < mapArr.GetLength(0); i++)
                {
                    if (mapArr[i, j] is Floor)
                        if (MapGeneration.Cell4W(mapArr, i, j, (int)MapTiles.Void) > 0) mapArr[i, j] = new Wall(new Vector2Int(i, j));
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
