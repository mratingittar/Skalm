using Skalm.Grid;
using Skalm.Maps.Tiles;
using Skalm.Structs;
using Skalm.Utilities;
using Skalm.Utilities.MapGeneration;
using System.Drawing;

namespace Skalm.Maps
{
    internal class MapGenerator
    {
        public HashSet<Vector2Int> FreeFloorTiles { get; private set; }
        public Map CurrentMap { get; private set; }

        private HashSet<Vector2Int> _floorTiles;
        private HashSet<Vector2Int> _doors;
        private Grid2D<BaseTile> _tileGrid;
        private List<Map> _mapList;
        private Map _starterMap;
        private int _mapIndex;
        private readonly ISettings _settings;

        private const int _tilesPerEnemy = 100;
        private const int _tilesPerItem = 200;
        private const int _tilesPerPotion = 150;

        public MapGenerator(Grid2D<BaseTile> tileGrid, ISettings settings)
        {
            _tileGrid = tileGrid;
            _settings = settings;
            _doors = new HashSet<Vector2Int>();
            _floorTiles = new HashSet<Vector2Int>();
            FreeFloorTiles = new HashSet<Vector2Int>();

            _mapIndex = 0;
            _mapList = new List<Map>();
            CurrentMap = _starterMap = LoadMapsFromFolder();
        }

        public Vector2Int GetRandomFloorPosition()
        {
            if (FreeFloorTiles.Count == 0)
                throw new Exception("No free tiles to spawn in found");

            Random rng = new Random();
            Vector2Int position = FreeFloorTiles.ElementAt(rng.Next(FreeFloorTiles.Count));
            FreeFloorTiles.Remove(position);
            return position;
        }

        public List<BaseTile> GetNeighbours(Vector2Int tile)
        {
            List<BaseTile> neighbors = new List<BaseTile>();
            if (_tileGrid.TryGetGridObject(tile.Add(new Vector2Int(0, -1)), out BaseTile up))
                neighbors.Add(up);
            if (_tileGrid.TryGetGridObject(tile.Add(new Vector2Int(1, 0)), out BaseTile right))
                neighbors.Add(right);
            if (_tileGrid.TryGetGridObject(tile.Add(new Vector2Int(0, 1)), out BaseTile down))
                neighbors.Add(down);
            if (_tileGrid.TryGetGridObject(tile.Add(new Vector2Int(-1, 0)), out BaseTile left))
                neighbors.Add(left);

            return neighbors;
        }

        public void NextLevel()
        {
            CurrentMap = LoadNextMap();
            CreateMap();
        }

        public void CreateMap()
        {
            CurrentMap = LoadMapIntoGrid(CurrentMap);
            FindWalls();
            SetBorderFloorsAsWalls();
        }

        public void ResetMapGenerator()
        {
            _mapIndex = 0;
            _starterMap.ResetMap();
            CurrentMap = _starterMap;
            _mapList.ForEach(m => m.ResetMap());
            RandomizeMaps();
        }

        public void RandomizeMaps()
        {
            _mapList.Shuffle();
            _mapList.ForEach(m => m.RandomModification());
        }

        public void ResetGrid()
        {
            _doors.Clear();
            _floorTiles.Clear();
            FreeFloorTiles.Clear();

            for (int x = 0; x < _tileGrid.gridWidth; x++)
            {
                for (int y = 0; y < _tileGrid.gridHeight; y++)
                {
                    _tileGrid.SetGridObject(x, y, new VoidTile(new Vector2Int(x, y)));
                }
            }
        }

        private Map LoadMapsFromFolder()
        {
            if (!FileHandler.TryReadFolder("maps", out List<string[]> maps))
                throw new Exception("No maps found in map folder!");

            Map starterMap = new Map(maps.First(), _settings.MapHeight, 1, 1, 1, 1);
            maps.RemoveAt(0);

            maps.ForEach(m => _mapList.Add(new Map(m, _settings.MapHeight, 5, 2, 0, 3)));
            
            RandomizeMaps();

            return starterMap;
        }

        private Map LoadNextMap()
        {
            if (_mapIndex >= _mapList.Count)
            {
                _mapIndex = 0;
                RandomizeMaps();
            }

            Map nextMap = _mapList[_mapIndex];
            _mapIndex++;
            return nextMap;
        }

        private Map LoadMapIntoGrid(Map map)
        {
            bool randomPlayerStart = true;
            bool randomPlayerGoal = true;

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
                            FreeFloorTiles.Add(new Vector2Int(x + startX, y + startY));
                            break;
                        case 'd':
                            CreateDoorTile(x + startX, y + startY);
                            break;
                        case 'e':
                            CreateFloorTile(x + startX, y + startY);
                            map.ObjectsInMap[EMapObjects.Enemies].Item2.Add(new Vector2Int(x + startX, y + startY));
                            break;
                        case 'i':
                            CreateFloorTile(x + startX, y + startY);
                            map.ObjectsInMap[EMapObjects.Items].Item2.Add(new Vector2Int(x + startX, y + startY));
                            break;
                        case 'k':
                            CreateFloorTile(x + startX, y + startY);
                            map.ObjectsInMap[EMapObjects.Keys].Item2.Add(new Vector2Int(x + startX, y + startY));
                            break;
                        case 'h':
                            CreateFloorTile(x + startX, y + startY);
                            map.ObjectsInMap[EMapObjects.Potions].Item2.Add(new Vector2Int(x + startX, y + startY));
                            break;
                        case 'p':
                            CreateFloorTile(x + startX, y + startY);
                            map.PlayerSpawnPosition = new Vector2Int(x + startX, y + startY);
                            randomPlayerStart = false;
                            break;
                        case 'g':
                            _tileGrid.SetGridObject(x + startX, y + startY, new FloorTile(new Vector2Int(x + startX, y + startY), _settings.GoalSprite, _settings.GoalColor, "stairs to the next floor"));
                            _floorTiles.Add(new Vector2Int(x + startX, y + startY));
                            map.GoalPosition = new Vector2Int(x + startX, y + startY);
                            randomPlayerGoal = false;
                            break;
                    }
                }
            }

            if (randomPlayerStart)
                map.PlayerSpawnPosition = GetRandomFloorPosition();

            if (randomPlayerGoal)
                map.GoalPosition = GetRandomFloorPosition();

            map.SetMininumObjectCount(_floorTiles.Count / _tilesPerEnemy, _floorTiles.Count / _tilesPerItem, _doors.Count, _floorTiles.Count / _tilesPerPotion);

            return map;
        }

        private void CreateDoorTile(int x, int y)
        {
            _tileGrid.SetGridObject(x, y, new DoorTile(new Vector2Int(x, y), _settings.DoorSpriteOpen, _settings.DoorSpriteClosed));
            _doors.Add(new Vector2Int(x, y));
        }

        private void CreateFloorTile(int x, int y)
        {
            _tileGrid.SetGridObject(x, y, new FloorTile(new Vector2Int(x, y), _settings.FloorSprite));
            _floorTiles.Add(new Vector2Int(x, y));
        }

        private void FindWalls()
        {
            HashSet<Vector2Int> tiles = _floorTiles.Union(_doors).ToHashSet();
            foreach (var tile in tiles)
            {
                List<BaseTile> neighbors = GetNeighbours(tile);
                foreach (var neighbor in neighbors)
                    if (neighbor is VoidTile)
                        _tileGrid.SetGridObject(neighbor.GridPosition, new WallTile(neighbor.GridPosition, _settings.WallSprite));
            }
        }

        private void SetBorderFloorsAsWalls()
        {
            foreach (var position in FreeFloorTiles)
            {
                if (position.X == 0
                    || position.X == _tileGrid.gridWidth - 1
                    || position.Y == 0
                    || position.Y == _tileGrid.gridHeight - 1)
                    _tileGrid.SetGridObject(position, new WallTile(position, _settings.WallSprite));
            }
        }

        private void GenerateRandomMap(int size = 42, int roomSize = 8, int maxRooms = 8)
        {
            Bounds map = new Bounds(new Vector2Int(1, 1), new Vector2Int(size - 1, size - 1));
            var BSPmap = BSPgen.BSPgeneration(map, roomSize, roomSize);
            var roomPos = BSPgen.FindRoomCenters(BSPmap);
            var maxMap = RoomGen.MaximumRoomsList(BSPmap);
            var padMap = BSPgen.AddPaddingToBoundsList(maxMap, 2);
            var rwMap = RoomGen.CreateRandomRoomsFromList(padMap, 0.65);
            var connMap = BSPgen.ConnectAllRooms(rwMap, roomPos);
        }
    }
}
