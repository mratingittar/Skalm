using Skalm.Grid;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Map
{
    internal class MapGenerator
    {
        public HashSet<Vector2Int> FloorTiles { get; private set; }
        public List<Vector2Int> EnemySpawnPositions { get; private set; }
        public List<Vector2Int> ItemSpawnPositions { get; private set; }
        public Vector2Int PlayerFixedSpawnPosition { get; private set; }

        private MapManager _mapManager;
        private HashSet<Vector2Int> _doors;
        private Grid2D<BaseTile> _tileGrid;
        private readonly ISettings _settings;

        public MapGenerator(MapManager mapManager, Grid2D<BaseTile> tileGrid, ISettings settings)
        {
            _mapManager = mapManager;
            _tileGrid = tileGrid;
            _settings = settings;
            _doors = new HashSet<Vector2Int>();
            FloorTiles = new HashSet<Vector2Int>();
            EnemySpawnPositions = new List<Vector2Int>();
            ItemSpawnPositions = new List<Vector2Int>();
            PlayerFixedSpawnPosition = Vector2Int.Zero;
        }

        public void CreateMap()
        {
            if (FileHandler.TryReadFile("map.txt", out string[] map))
            {

                CreateMapFromStringArray(map);
            }
            FindWalls();
            SetBorderFloorsAsWalls();

            if (FloorTiles.Count == 0)
                CreateRoomFromBounds(new Bounds(new Vector2Int(5, 5), new Vector2Int(_tileGrid.gridWidth - 5, _tileGrid.gridHeight - 5)));
        }

        private void CreateMapFromStringArray(string[] map)
        {
            if (map.Length == 0 || map.Min() == null)
                throw new ArgumentException("map file is empty");

            int mapHeight = Math.Min(map.Length, _settings.MapHeight);
            int mapWidth = Math.Min(map.Select(s => s.Length).Max(), _settings.MapWidth);

            int startX = 0;
            int startY = 0;

            if (mapWidth < _settings.MapWidth)
                startX = (_settings.MapWidth - mapWidth) / 2;

            if (mapHeight < _settings.MapHeight)
                startY = (_settings.MapHeight - mapHeight) / 2;

            for (int y = 0; y < mapHeight; y++)
            {
                int width = Math.Min(map[y].Length, _settings.MapWidth);
                for (int x = 0; x < width; x++)
                {
                    switch (map[y][x])
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
                        case 'p':
                            CreateFloorTile(x + startX, y + startY);
                            PlayerFixedSpawnPosition = new Vector2Int(x + startX, y + startY);
                            break;
                    }
                }
            }
        }

        private void CreateDoorTile(int x, int y)
        {
            _tileGrid.SetGridObject(x, y, new DoorTile(new Vector2Int(x, y), _settings.SpriteDoorOpen, _settings.SpriteDoorClosed));
            _doors.Add(new Vector2Int(x, y));
        }

        private void CreateFloorTile(int x, int y)
        {
            _tileGrid.SetGridObject(x, y, new FloorTile(new Vector2Int(x, y), _settings.SpriteFloor));
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
                        _tileGrid.SetGridObject(neighbor.GridPosition, new WallTile(neighbor.GridPosition, _settings.SpriteWall));
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
                    _tileGrid.SetGridObject(position, new WallTile(position, _settings.SpriteWall));
            }
        }


        private string[] FlipMapHorizontal(string[] mapInput)
        {
            mapInput = PadStringsInArrayToEqualLength(mapInput);
            if (mapInput.Max() is null)
                return mapInput;

            for (int i = 0; i < mapInput.Length; i++)
            {
                mapInput[i] = new string(mapInput[i].Reverse().ToArray());
            }

            return mapInput;
        }

        private string[] FlipMapVertical(string[] mapInput)
        {
            mapInput = PadStringsInArrayToEqualLength(mapInput);
            if (mapInput.Max() is null)
                return mapInput;

            string[] mapOutput = new string[mapInput.Length];
            for (int i = 0; i < mapInput.Length; i++)
            {
                mapOutput[^(i + 1)] = mapInput[i];
            }

            return mapOutput;
        }

        private string[] RotateMap(string[] mapInput, bool clockwise)
        {
            mapInput = PadStringsInArrayToEqualLength(mapInput);

            if (mapInput.First().Length != mapInput.Length)
                mapInput = SquareStringArray(mapInput, _settings.MapHeight);

            int size = mapInput.Length;

            char[,] charMatrix = new char[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    charMatrix[i, j] = mapInput[i][j];
                }
            }
            char[,] rotated = new char[size, size];

            if (clockwise)
                rotated = RotateCharMatrixClockwise(charMatrix, size);
            else
                rotated = RotateCharMatrixCounterClockwise(charMatrix, size);

            string[] mapOutput = new string[size];

            for (int k = 0; k < size; k++)
            {
                for (int l = 0; l < size; l++)
                {
                    mapOutput[k] += rotated[k, l];
                }
            }

            return mapOutput;
        }

        private string[] SquareStringArray(string[] input, int limit)
        {
            int height = input.Length;
            int width = input.First().Length;

            // Pad width to match height
            if (height > width)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    input[i].PadRight(limit, ' ');
                }
            }
            // Pad height to match width
            else if (width > height)
            {
                for (int i = 0; i < width - height; i++)
                {
                    input.Append("");
                }
            }

            // Trim width to match limit
            if (input.First().Length > limit)
                input = input.Select(s => s.Remove(limit)).ToArray();

            // Trim height to match limit
            if (input.Length > limit)
                input = input.Take(limit).ToArray();

            return input;
        }

        private char[,] RotateCharMatrixClockwise(char[,] matrix, int n)
        {
            char[,] result = new char[n, n];

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    result[i, j] = matrix[n - j - 1, i];
                }
            }

            return result;
        }

        private char[,] RotateCharMatrixCounterClockwise(char[,] matrix, int n)
        {
            char[,] result = new char[n, n];

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    result[i, j] = matrix[j, n - i - 1];
                }
            }

            return result;
        }

        private string[] PadStringsInArrayToEqualLength(string[] input)
        {
            int maxWidth = input.Select(s => s.Length).Max();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Length < maxWidth)
                    input[i] = input[i].PadRight(maxWidth, ' ');
            }
            return input;
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
