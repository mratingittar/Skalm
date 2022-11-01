using Skalm.Structs;

namespace Skalm.Maps
{
    internal class StringMap : IMap
    {
        public string[] MapString { get; private set; }
        public Dictionary<EMapObjects, (int, List<Vector2Int>)> ObjectsInMap { get; private set; }
        public HashSet<Vector2Int> FloorTiles { get; set; }
        public HashSet<Vector2Int> DoorTiles { get; set; }
        public Vector2Int PlayerSpawnPosition { get; set; }
        public Vector2Int GoalPosition { get; set; }

        private int _width => MapString.Select(s => s.Length).Max();
        private int _height => MapString.Length;
        private int _limit;
        public StringMap(string[] mapString, int sizeLimit, int enemies, int items, int keys, int potions)
        {
            _limit = sizeLimit;
            MapString = PadStringsInArrayToEqualLength(mapString);
            ObjectsInMap = new Dictionary<EMapObjects, (int, List<Vector2Int>)>
            {
                { EMapObjects.Enemies, (enemies,new List<Vector2Int>()) },
                { EMapObjects.Items, (items,new List<Vector2Int>()) },
                { EMapObjects.Keys, (keys,new List<Vector2Int>()) },
                { EMapObjects.Potions, (potions,new List<Vector2Int>()) }
            };
            FloorTiles = new HashSet<Vector2Int>();
            DoorTiles = new HashSet<Vector2Int>();
        }

        public void SetMininumObjectCount(int enemies, int items, int keys, int potions)
        {
            ObjectsInMap[EMapObjects.Enemies] = (enemies, ObjectsInMap[EMapObjects.Enemies].Item2);
            ObjectsInMap[EMapObjects.Items] = (items, ObjectsInMap[EMapObjects.Items].Item2);
            ObjectsInMap[EMapObjects.Keys] = (keys, ObjectsInMap[EMapObjects.Keys].Item2);
            ObjectsInMap[EMapObjects.Potions] = (potions, ObjectsInMap[EMapObjects.Potions].Item2);
        }

        public void ResetMap()
        {
            foreach (KeyValuePair<EMapObjects, (int, List<Vector2Int>)> item in ObjectsInMap)
            {
                item.Value.Item2.Clear();
            }

            FloorTiles.Clear();
            DoorTiles.Clear();
        }

        public void RandomModification()
        {
            Random rng = new Random();
            switch (rng.Next(4))
            {
                case 0:
                    FlipMapHorizontal();
                    break;
                case 1:
                    FlipMapVertical();
                    break;
                case 2:
                    RotateMap(true);
                    break;
                case 3:
                    RotateMap(false);
                    break;
                default:
                    break;
            }
        }

        public void FlipMapHorizontal()
        {
            for (int i = 0; i < _height; i++)
            {
                MapString[i] = new string(MapString[i].Reverse().ToArray());
            }
        }

        public void FlipMapVertical()
        {
            string[] flip = new string[_height];
            for (int i = 0; i < _height; i++)
            {
                flip[^(i + 1)] = MapString[i];
            }

            MapString = flip;
        }

        public void RotateMap(bool clockwise)
        {
            string[] mapInput = MapString;
            mapInput = SquareStringArray(mapInput, _limit);
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

            MapString = mapOutput;
        }

        private string[] SquareStringArray(string[] input, int limit)
        {
            int height = input.Length;
            int width = input.First().Length;

            if (width > limit)
                input = input.Select(s => s.Remove(limit)).ToArray();

            if (height > limit)
                input = input.Take(limit).ToArray();

            if (width > height)
            {
                int difference = width - height;
                int above = difference / 2;
                int below = difference - above;

                List<string> stringList = input.ToList();
                for (int i = 0; i < above; i++)
                {
                    stringList.Insert(0, "".PadRight(width));
                }

                for (int i = 0; i < below; i++)
                {
                    stringList.Add("".PadRight(width));
                }

                input = stringList.ToArray();
            }

            if (height > width)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    input[i] = input[i].PadLeft(height / 2 + input[i].Length / 2);
                    input[i] = input[i].PadRight(height);
                }
            }

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
            input = input.Select(s => s.TrimEnd()).ToArray();
            int maxWidth = input.Select(s => s.Length).Max();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Length < maxWidth)
                    input[i] = input[i].PadRight(maxWidth, ' ');
            }
            return input;
        }
    }
}
