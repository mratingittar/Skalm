using Skalm.Actors.Tile;
using Skalm.Display;
using Skalm.Grid;
using Skalm.Structs;

namespace Skalm.Map
{
    internal class MapManager
    {
        public readonly Grid2D<BaseTile> tileGrid;

        public readonly DisplayManager displayManager;
        public readonly MapPrinter mapPrinter;

        //private List<Tile> tileBase;

        public MapManager(Grid2D<BaseTile> tileGrid, DisplayManager displayManager)
        {
            this.tileGrid = tileGrid;
            this.displayManager = displayManager;
            this.mapPrinter = new MapPrinter(this, displayManager);

            //this.tileBase = new() {new Tile()};
        }

        // DRAW ALL TILES


        // METHOD CREATE MAP
        public void CreateMap()
        {
            CreateRoom(new Bounds(new Vector2Int(5, 5), new Vector2Int(tileGrid.gridWidth - 10, tileGrid.gridHeight - 10)));
        }

        // METHOD CREATE ROOM
        private void CreateRoom(Bounds roomSpace)
        {
            // CREATE WALLS
            CreateRoomWalls(roomSpace);

            // CREATE FLOOR
            for (int j = roomSpace.StartXY.Y + 1; j <= roomSpace.EndXY.Y - 1; j++)
            {
                for (int i = roomSpace.StartXY.X + 1; i <= roomSpace.EndXY.X - 1; i++)
                {
                    tileGrid.SetGridObject(i, j, new FloorTile(new Vector2Int(i, j)));
                }
            }
        }

        // METHOD CREATE ROOM WALLS
        private void CreateRoomWalls(Bounds roomSpace)
        {
            int posX1, posX2, posY1, posY2;

            // HORIZONTAL AXIS
            for (int i = roomSpace.StartXY.X; i <= roomSpace.EndXY.X; i++)
            {
                posY1 = roomSpace.StartXY.Y;
                posY2 = roomSpace.EndXY.Y;

                tileGrid.SetGridObject(i, posY1, new WallTile(new Vector2Int(i, posY1)));
                tileGrid.SetGridObject(i, posY2, new WallTile(new Vector2Int(i, posY2)));
            }

            // VERTICAL AXIS
            for (int j = roomSpace.StartXY.Y; j <= roomSpace.EndXY.Y; j++)
            {
                posX1 = roomSpace.StartXY.X;
                posX2 = roomSpace.EndXY.X;

                tileGrid.SetGridObject(posX1, j, new WallTile(new Vector2Int(posX1, j)));
                tileGrid.SetGridObject(posX2, j, new WallTile(new Vector2Int(posX1, j)));
            }
        }


        //    private Vector2Int mapOrigin;
        //    private int mapWidth;
        //    private int mapHeight;
        //    //public Grid<Cell> mapGrid;
        //    public int[,] mapArr { get; private set; }

        //    // CONSTRUCTOR I
        //    public MapManager(int width, int height, Vector2Int origin)
        //    {
        //        mapWidth = width;
        //        mapHeight = height;
        //        mapOrigin = origin;
        //        //mapGrid = new Grid<Cell>(width, height, 2, 1, origin, (position, x, y) => new Cell(position, x, y));


        //        mapArr = new int[mapWidth, mapHeight];
        //        //InitMap();
        //    }

        //    // METHOD INITIALIZE MAP
        //    public void InitMap()
        //    {
        //        for (int j = 4; j < mapArr.GetLength(1) - 4; j++)
        //        {
        //            for (int i = 4; i < mapArr.GetLength(0) - 4; i++)
        //            {
        //                mapArr[i, j] = (int)MapTiles.Floor;
        //            }
        //        }

        //        FindWalls();
        //    }

        //    // METHOD CELL VON NEUMAN
        //    private int cell4W(int x, int y, int value)
        //    {
        //        int output = 0;
        //        if (mapArr[x, y - 1] == value) output++;
        //        if (mapArr[x, y + 1] == value) output++;
        //        if (mapArr[x - 1, y] == value) output++;
        //        if (mapArr[x + 1, y] == value) output++;
        //        return output;
        //    }

        //    // FIND WALLS IN ARRAY
        //    private void FindWalls()
        //    {
        //        for (int j = 0; j < mapArr.GetLength(1); j++)
        //        {
        //            for (int i = 0; i < mapArr.GetLength(0); i++)
        //            {
        //                if (mapArr[i, j] == (int)MapTiles.Floor)
        //                    if (cell4W(i, j, (int)MapTiles.Void) > 0) mapArr[i, j] = (int)MapTiles.Wall;
        //            }
        //        }
        //    }
        //}

        public enum MapTiles
        {
            Void,
            Wall,
            Floor,
            Door
        }
    }
}
