namespace Skalm.Map
{
    internal class Map
    {
        public string[] MapString { get; private set; }
        public int Width { get => MapString.Select(s => s.Length).Max(); }
        public int Height { get => MapString.Length; }
        private int _limit;
        public Map(string[] mapString, int mapLimit)
        {
            MapString = PadStringsInArrayToEqualLength(mapString);
            _limit = mapLimit;
        }

        public void FlipMapHorizontal()
        {
            for (int i = 0; i < Height; i++)
            {
                MapString[i] = new string(MapString[i].Reverse().ToArray());
            }
        }

        public void FlipMapVertical()
        {
            string[] flip = new string[Height];
            for (int i = 0; i < Height; i++)
            {
                flip[^(i + 1)] = MapString[i];
            }

            MapString = flip;
        }

        public void RotateMap(bool clockwise)
        {
            string[] mapInput = MapString;

            if (mapInput.First().Length != mapInput.Length)
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

            // Match width to limit
            if (width < limit)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    input[i].PadRight(limit, ' ');
                }
            }
            else if (width > limit)
                input = input.Select(s => s.Remove(limit)).ToArray();

            // Match height to limit
            if (height < limit)
            {
                for (int i = 0; i < limit - height; i++)
                {
                    input = input.Append("".PadRight(limit)).ToArray();
                }
            }
            else if (height > limit)
                input = input.Take(limit).ToArray();

            return input;
        }

        private string EmptyStringOfLength(int length)
        {
            string empty = "";
            for (int i = 0; i < length; i++)
            {
                empty += " ";
            }
            return empty;
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
