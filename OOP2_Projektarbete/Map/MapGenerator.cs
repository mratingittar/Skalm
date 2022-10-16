using Skalm.Actors.Tile;
using Skalm.Grid;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Map
{
    internal class MapGenerator
    {
        private ISettings settings;
        private Grid2D<BaseTile> tileGrid;
        private HashSet<Vector2Int> freeTiles;
        private HashSet<Vector2Int> doors;

        public MapGenerator(Grid2D<BaseTile> tileGrid, ISettings settings)
        {
            this.tileGrid = tileGrid;
            freeTiles = new HashSet<Vector2Int>();
            doors = new HashSet<Vector2Int>();
            this.settings = settings;
        }

        public void CreateMap()
        {
            if (FileHandler.TryReadFile("map.txt", out string[] map))
                CreateMapFromStringArray(map);
            FindWalls();
            SetBorderFloorsAsWalls();

            if (freeTiles.Count == 0)
                CreateRoomFromBounds(new Bounds(new Vector2Int(5, 5), new Vector2Int(tileGrid.gridWidth - 5, tileGrid.gridHeight - 5)));
        }

        public Vector2Int GetRandomSpawnPosition()
        {
            if (freeTiles.Count == 0)
                throw new Exception("No free tiles to spawn in found");

            Random randomPos = new Random();
            return freeTiles.ElementAt(randomPos.Next(freeTiles.Count()));
        }

        private void CreateMapFromStringArray(string[] map)
        {
            if (map.Length == 0)
                throw new ArgumentException("map file is empty");

            int height = Math.Min(map.Length, settings.MapHeight);

            for (int y = 0; y < height; y++)
            {
                int width = Math.Min(map[y].Length, settings.MapWidth);
                for (int x = 0; x < width; x++)
                {
                    if (map[y][x] == 'f')
                    {
                        tileGrid.SetGridObject(x, y, new FloorTile(new Vector2Int(x, y), settings.SpriteFloor));
                        freeTiles.Add(new Vector2Int(x, y));
                    }

                    else if (map[y][x] == 'd')
                    {
                        tileGrid.SetGridObject(x, y, new DoorTile(new Vector2Int(x, y), settings.SpriteDoorOpen, settings.SpriteDoorClosed));
                        doors.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        private void FindWalls()
        {
            HashSet<Vector2Int> tiles = freeTiles.Union(doors).ToHashSet();
            foreach (var tile in tiles)
            {
                List<BaseTile> neighbors = GetNeighbours(tile);
                foreach (var neighbor in neighbors)
                    if (neighbor is VoidTile)
                        tileGrid.SetGridObject(neighbor.GridPosition, new WallTile(neighbor.GridPosition, settings.SpriteWall));
            }
        }

        private List<BaseTile> GetNeighbours(Vector2Int tile)
        {
            List<BaseTile> neighbors = new List<BaseTile>();
            if (tileGrid.TryGetGridObject(tile.Add(new Vector2Int(0, -1)), out BaseTile up))
                neighbors.Add(up);
            if (tileGrid.TryGetGridObject(tile.Add(new Vector2Int(1, 0)), out BaseTile right))
                neighbors.Add(right);
            if (tileGrid.TryGetGridObject(tile.Add(new Vector2Int(0, 1)), out BaseTile down))
                neighbors.Add(down);
            if (tileGrid.TryGetGridObject(tile.Add(new Vector2Int(-1, 0)), out BaseTile left))
                neighbors.Add(left);

            return neighbors;
        }

        private void SetBorderFloorsAsWalls()
        {
            foreach (var position in freeTiles)
            {
                if (position.X == 0
                    || position.X == tileGrid.gridWidth - 1
                    || position.Y == 0
                    || position.Y == tileGrid.gridHeight -1 )
                    tileGrid.SetGridObject(position, new WallTile(position, settings.SpriteWall));
            }
        }

        private void CreateRoomFromBounds(Bounds roomSpace)
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
    }
}
