using Skalm.Grid;
using Skalm.Maps.ProceduralGeneration;
using Skalm.Maps.Tiles;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Maps
{
    internal class MapGenerator
    {
        private const int _tilesPerEnemy = 100;
        private const int _tilesPerItem = 200;
        private const int _tilesPerPotion = 150;

        public HashSet<Vector2Int> FreeFloorTiles { get; private set; }
        public IMap CurrentMap { get; private set; }
        public bool UseRandomMaps { set => _useRandomMaps = value; }

        private HashSet<Vector2Int> _floorTiles;
        private HashSet<Vector2Int> _doors;
        private Grid2D<BaseTile> _tileGrid;
        private List<StringMap> _mapList;
        private StringMap _starterMap;
        private int _mapIndex;
        private readonly ISettings _settings;
        private bool _useRandomMaps;


        // CONSTRUCTOR I
        public MapGenerator(Grid2D<BaseTile> tileGrid, ISettings settings)
        {
            _tileGrid = tileGrid;
            _settings = settings;
            _doors = new HashSet<Vector2Int>();
            _floorTiles = new HashSet<Vector2Int>();
            FreeFloorTiles = new HashSet<Vector2Int>();

            _mapIndex = 0;
            _mapList = new List<StringMap>();
            CurrentMap = _starterMap = LoadMapsFromFolder();
        }

        // GET RANDOM FLOOR POSITION
        public Vector2Int GetRandomFloorPosition()
        {
            if (FreeFloorTiles.Count == 0)
                throw new Exception("No free tiles to spawn in found");

            Random rng = new Random();
            Vector2Int position = FreeFloorTiles.ElementAt(rng.Next(FreeFloorTiles.Count));
            FreeFloorTiles.Remove(position);
            return position;
        }

        // GET NEIGHBORS
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

        // NEXT LEVEL
        public void NextLevel()
        {
            CurrentMap = _useRandomMaps ? GenerateRandomMap() : LoadNextStringMap();
            CreateMap();
        }

        // CREATE MAP
        public void CreateMap()
        {
            if (_useRandomMaps)
            {
                CurrentMap = GenerateRandomMap();
                LoadRandomMapIntoGrid((RandomMap)CurrentMap);
            }
            else
            {
                CurrentMap = LoadStringMapIntoGrid((StringMap)CurrentMap);
            }
            FindWalls();
            SetBorderFloorsAsWalls();
        }

        // RESET MAP GENERATOR
        public void ResetMapGenerator()
        {
            _mapIndex = 0;
            _starterMap.ResetMap();
            CurrentMap = _starterMap;
            _mapList.ForEach(m => m.ResetMap());
            RandomizeMaps();
        }

        // RANDOMIZE MAPS
        public void RandomizeMaps()
        {
            _mapList.Shuffle();
            _mapList.ForEach(m => m.RandomModification());
            _mapList.ForEach(m => m.FloorTiles.Clear());
            _mapList.ForEach(m => m.DoorTiles.Clear());
        }

        // RESET GRID
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

        private StringMap LoadMapsFromFolder()
        {
            if (!FileHandler.TryReadFolder("maps", out List<string[]> maps))
                throw new Exception("No maps found in map folder!");

            StringMap starterMap = new StringMap(maps.First(), _settings.MapHeight, 1, 1, 1, 1);
            maps.RemoveAt(0);

            maps.ForEach(m => _mapList.Add(new StringMap(m, _settings.MapHeight, 5, 2, 0, 3)));

            RandomizeMaps();

            return starterMap;
        }

        private StringMap LoadNextStringMap()
        {
            if (_mapIndex >= _mapList.Count)
            {
                _mapIndex = 0;
                RandomizeMaps();
            }

            StringMap nextMap = _mapList[_mapIndex];
            _mapIndex++;
            return nextMap;
        }

        private StringMap LoadStringMapIntoGrid(StringMap map)
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
                map.GoalPosition = MapGen.FindFurthestVectorInList(map.PlayerSpawnPosition, FreeFloorTiles.ToList());
            //GetRandomFloorPosition();

            map.SetMininumObjectCount(_floorTiles.Count / _tilesPerEnemy, _floorTiles.Count / _tilesPerItem, _doors.Count, _floorTiles.Count / _tilesPerPotion);
            map.FloorTiles = _floorTiles;
            map.DoorTiles = _doors;

            return map;
        }

        // CREATE DOOR TILE
        private void CreateDoorTile(int x, int y)
        {
            _tileGrid.SetGridObject(x, y, new DoorTile(new Vector2Int(x, y), _settings.DoorSpriteOpen, _settings.DoorSpriteClosed));
            _doors.Add(new Vector2Int(x, y));
        }

        // CREATE FLOOR TILE
        private void CreateFloorTile(int x, int y)
        {
            _tileGrid.SetGridObject(x, y, new FloorTile(new Vector2Int(x, y), _settings.FloorSprite));
            _floorTiles.Add(new Vector2Int(x, y));
        }

        // FIND WALLS
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

        // SET BORDER FLOORS AS WALLS
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

        // GENERATE MAP PROCEDURALLY
        private RandomMap GenerateRandomMap(int size = 42, int roomSize = 9, int maxRooms = 8)
        {
            // BOUNDS FOR WHOLE MAP
            Bounds map = new Bounds(new Vector2Int(1, 1), new Vector2Int(size - 1, size - 1));

            // BINARY SPACE PARTITIONING OF MAP BOUNDS
            var BSPmap = BSPgen.BSPgeneration(map, roomSize, roomSize);

            // FIND EACH PARTITIONED ROOM CENTER POSITIONS
            var roomPos = BSPgen.FindRoomCenters(BSPmap);

            // LIMIT ROOMS TO CERTAIN MAXIMUM
            var maxMap = RoomGen.MaximumRoomsList(BSPmap);

            // REMOVE ROOM CENTERS FOR REMOVED ROOMS
            roomPos = RoomGen.RemoveEmptyRoomCenters(maxMap, roomPos);

            // PAD ALL ROOM BOUNDS = SMALLER ROOMS
            var padMap = BSPgen.AddPaddingToBoundsList(maxMap, 2);
            //var padMap = BSPgen.AddRandomPaddingToBoundsList(maxMap, 3);
            //var rwMap = RandomWalkGen.RandomWalkBoundsList(padMap);

            // CREATE ACTUAL ROOMS OF RANDOM TYPES, EXPORT TILES COLLECTION
            var rwMap = RoomGen.CreateRandomRoomsFromList(padMap, 0.8);

            // CONNECT ALL ROOMS WITH CORRIDORS AND FIND DOORS
            var connMap = BSPgen.ConnectAllRooms(rwMap, roomPos, maxMap);
            var doorList = connMap.Item2;
            var floorTiles = connMap.Item1;

            // MAKE SURE ALL ROOM CENTERS ARE 3x3 FLOOR
            floorTiles.UnionWith(RoomGen.FillAroundRoomCenters(roomPos));

            // TILE LISTS BY TYPE
            var doorList3 = MapGen.DoorCleaner(doorList, floorTiles);
            var doorList2 = BSPgen.FindDoorsFromBoundsList(maxMap, floorTiles);
            var wallList = RoomGen.FindAllWalls(floorTiles);

            _floorTiles = floorTiles;
            _doors = doorList3;
            FreeFloorTiles = floorTiles;
            RandomMap randomMap = new RandomMap(_floorTiles, _doors,
                _floorTiles.Count / _tilesPerEnemy, _floorTiles.Count / _tilesPerItem, _doors.Count, _floorTiles.Count / _tilesPerPotion);

            List<Vector2Int> roomCenters = roomPos;
            roomCenters = PopRandomPositionInList(roomCenters, out Vector2Int playerPos);
            randomMap.PlayerSpawnPosition = playerPos;
            randomMap.GoalPosition = MapGen.FindFurthestVectorInList(playerPos, roomCenters); ;
            return randomMap;
        }

        private List<Vector2Int> PopRandomPositionInList(List<Vector2Int> positions, out Vector2Int pos)
        {
            Random rng = new Random();
            pos = positions.ElementAt(rng.Next(positions.Count));
            positions.Remove(pos);
            return positions;
        }

        private void LoadRandomMapIntoGrid(RandomMap map)
        {
            foreach (var floor in map.FloorTiles)
            {
                CreateFloorTile(floor.X, floor.Y);
            }
            foreach (var door in map.DoorTiles)
            {
                CreateDoorTile(door.X, door.Y);
            }
            _tileGrid.SetGridObject(map.GoalPosition, new FloorTile(
                map.GoalPosition, _settings.GoalSprite, _settings.GoalColor, "stairs to the next floor"));

        }
    }
}
