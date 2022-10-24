using Skalm.Display;
using Skalm.Grid;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Map
{
    internal class MapGenerator
    {
        public HashSet<Vector2Int> FloorTiles { get; private set; }
        public HashSet<Vector2Int> Doors { get => _doors; }
        public List<Vector2Int> EnemySpawnPositions { get; private set; }
        public List<Vector2Int> ItemSpawnPositions { get; private set; }
        public List<Vector2Int> PotionSpawnPositions { get; private set; }
        public List<Vector2Int> KeySpawnPositions { get; private set; }
        public Vector2Int PlayerFixedSpawnPosition { get; private set; }
        public Vector2Int GoalPosition { get; private set; }

        private MapManager _mapManager;
        private DisplayManager _displayManager;
        private HashSet<Vector2Int> _doors;
        private Grid2D<BaseTile> _tileGrid;
        private Dictionary<int, Map> _maps;
        private int _mapIndex;
        private readonly ISettings _settings;

        public MapGenerator(MapManager mapManager, DisplayManager displayManager, Grid2D<BaseTile> tileGrid, ISettings settings)
        {
            _mapManager = mapManager;
            _displayManager = displayManager;
            _tileGrid = tileGrid;
            _settings = settings;
            _doors = new HashSet<Vector2Int>();
            _maps = new Dictionary<int, Map>();
            _mapIndex = 0;
            FloorTiles = new HashSet<Vector2Int>();
            EnemySpawnPositions = new List<Vector2Int>();
            ItemSpawnPositions = new List<Vector2Int>();
            PotionSpawnPositions = new List<Vector2Int>();
            KeySpawnPositions = new List<Vector2Int>();
            PlayerFixedSpawnPosition = Vector2Int.Zero;
            GoalPosition = Vector2Int.Zero;

            LoadMapsFromFolder();
        }

        private void LoadMapsFromFolder()
        {
            if (FileHandler.TryReadFolder("maps", out List<string[]> mapList))
            {
                int counter = 0;
                foreach (var map in mapList)
                {
                    _maps.Add(counter, new Map(map, _settings.MapHeight));
                    counter++;
                }
            }
        }

        public void ResetMapIndex() => _mapIndex = 0;

        public void CreateMap()
        {
            LoadMapIntoGrid(LoadNextMap()); //Create failsafe if map folder is empty
            FindWalls();
            SetBorderFloorsAsWalls();

            GoalPosition = GoalPosition.Equals(Vector2Int.Zero)
                ? _mapManager.GetRandomFloorPosition() : GoalPosition;
        }

        public void ResetMap()
        {
            _doors.Clear();
            FloorTiles.Clear();
            EnemySpawnPositions.Clear();
            ItemSpawnPositions.Clear();
            PotionSpawnPositions.Clear();
            KeySpawnPositions.Clear();
            PlayerFixedSpawnPosition = Vector2Int.Zero;
            GoalPosition = Vector2Int.Zero;

            for (int x = 0; x < _tileGrid.gridWidth; x++)
            {
                for (int y = 0; y < _tileGrid.gridHeight; y++)
                {
                    _tileGrid.SetGridObject(x,y, new VoidTile(new Vector2Int(x,y)));
                }
            }
        }
        private Map LoadNextMap()
        {
            if (_mapIndex >= _maps.Count)
                _mapIndex = 0;

            Map nextMap = _maps[_mapIndex];
            _mapIndex++;
            return nextMap;
        }

        private void LoadMapIntoGrid(Map map)
        {
            string[] mapArray = map.MapString;
            if (mapArray.Length == 0 || mapArray.Min() == null)
                throw new ArgumentException("map file is empty");

            int mapHeight = Math.Min(mapArray.Length, _settings.MapHeight);
            int mapWidth = Math.Min(mapArray.Select(s => s.Length).Max(), _settings.MapWidth);

            int startX = 0;
            int startY = 0;

            if (mapWidth < _settings.MapWidth)
                startX = (_settings.MapWidth - mapWidth) / 2;

            if (mapHeight < _settings.MapHeight)
                startY = (_settings.MapHeight - mapHeight) / 2;

            for (int y = 0; y < mapHeight; y++)
            {
                int width = Math.Min(mapArray[y].Length, _settings.MapWidth);
                for (int x = 0; x < width; x++)
                {
                    switch (mapArray[y][x])
                    {
                        case 'f':
                            CreateFloorTile(x + startX, y + startY);
                            break;
                        case 'd':
                            CreateDoorTile(x + startX, y + startY);
                            break;
                        case 'e':
                            CreateFloorTile(x + startX, y + startY);
                            EnemySpawnPositions.Add(new Vector2Int(x + startX, y + startY));
                            break;
                        case 'i':
                            CreateFloorTile(x + startX, y + startY);
                            ItemSpawnPositions.Add(new Vector2Int(x + startX, y + startY));
                            break;
                        case 'k':
                            CreateFloorTile(x + startX, y + startY);
                            KeySpawnPositions.Add(new Vector2Int(x + startX, y + startY));
                            break;
                        case 'h':
                            CreateFloorTile(x + startX, y + startY);
                            PotionSpawnPositions.Add(new Vector2Int(x + startX, y + startY));
                            break;
                        case 'p':
                            CreateFloorTile(x + startX, y + startY);
                            PlayerFixedSpawnPosition = new Vector2Int(x + startX, y + startY);
                            break;
                        case 'g':
                            _tileGrid.SetGridObject(x + startX, y + startY, new FloorTile(new Vector2Int(x + startX, y + startY), _settings.GoalSprite, _settings.GoalColor, "stairs to the next floor"));
                            FloorTiles.Add(new Vector2Int(x + startX, y + startY));
                            GoalPosition = new Vector2Int(x + startX, y + startY);
                            break;
                    }
                }
            }
        }

        private void CreateDoorTile(int x, int y)
        {
            _tileGrid.SetGridObject(x, y, new DoorTile(new Vector2Int(x, y), _settings.DoorSpriteOpen, _settings.DoorSpriteClosed));
            _doors.Add(new Vector2Int(x, y));
        }

        private void CreateFloorTile(int x, int y)
        {
            _tileGrid.SetGridObject(x, y, new FloorTile(new Vector2Int(x, y), _settings.FloorSprite));
            FloorTiles.Add(new Vector2Int(x, y));
        }

        private void FindWalls()
        {
            HashSet<Vector2Int> tiles = FloorTiles.Union(_doors).ToHashSet();
            foreach (var tile in tiles)
            {
                List<BaseTile> neighbors = _mapManager.GetNeighbours(tile);
                foreach (var neighbor in neighbors)
                    if (neighbor is VoidTile)
                        _tileGrid.SetGridObject(neighbor.GridPosition, new WallTile(neighbor.GridPosition, _settings.WallSprite));
            }
        }

        private void SetBorderFloorsAsWalls()
        {
            foreach (var position in FloorTiles)
            {
                if (position.X == 0
                    || position.X == _tileGrid.gridWidth - 1
                    || position.Y == 0
                    || position.Y == _tileGrid.gridHeight - 1)
                    _tileGrid.SetGridObject(position, new WallTile(position, _settings.WallSprite));
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
                    FloorTiles.Add(new Vector2Int(i, j));
                    _tileGrid.SetGridObject(i, j, new FloorTile(new Vector2Int(i, j)));
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

                _tileGrid.SetGridObject(i, posY1, new WallTile(new Vector2Int(i, posY1)));
                _tileGrid.SetGridObject(i, posY2, new WallTile(new Vector2Int(i, posY2)));
            }

            // VERTICAL AXIS
            for (int j = roomSpace.StartXY.Y; j <= roomSpace.EndXY.Y; j++)
            {
                posX1 = roomSpace.StartXY.X;
                posX2 = roomSpace.EndXY.X;

                _tileGrid.SetGridObject(posX1, j, new WallTile(new Vector2Int(posX1, j)));
                _tileGrid.SetGridObject(posX2, j, new WallTile(new Vector2Int(posX1, j)));
            }
        }
    }
}
