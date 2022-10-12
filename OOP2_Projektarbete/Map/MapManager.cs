using Skalm.Actors;
using Skalm.Actors.Player;
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

        private HashSet<Vector2Int> freeTiles;
        public List<IGameObject> gameObjects;

        Player player;

        // CONSTRUCTOR I
        public MapManager(Grid2D<BaseTile> tileGrid, DisplayManager displayManager)
        {
            this.tileGrid = tileGrid;
            this.displayManager = displayManager;
            this.mapPrinter = new MapPrinter(this, displayManager);

            freeTiles = new HashSet<Vector2Int>();
            gameObjects = new List<IGameObject>();

            player = new Player(tileGrid, new Vector2Int(tileGrid.gridWidth / 2, tileGrid.gridHeight / 2), new Actors.Player.PlayerMoveInput(), new Actors.Player.PlayerAttackComponent());
        }

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
                    freeTiles.Add(new Vector2Int(i, j));
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

        public enum MapTiles
        {
            Void,
            Wall,
            Floor,
            Door
        }
    }
}
