using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Structs
{
    internal struct Vector2Int
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector2Int(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Vector2Int Zero { get => new Vector2Int(0, 0); }

        public static bool Equals(Vector2Int a, Vector2Int b)
        {
            return ((a.X == b.X) && (a.Y == b.Y));
        }
    }
}
