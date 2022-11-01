using Skalm.Structs;


namespace Skalm.Maps.ProceduralGeneration
{
    // RANDOM WALK GENERATOR
    internal static class RWgen
    {
        static Random rng = new Random();

        // CARDINAL DIRS
        public static List<Vector2Int> cardinalDirs = new List<Vector2Int>
        {
            Vector2Int.Up,
            Vector2Int.Left,
            Vector2Int.Down,
            Vector2Int.Right,
        };

        // GET RANDOM CARDINAL DIR
        public static Vector2Int Rnd4Dir() => cardinalDirs[rng.Next(0, cardinalDirs.Count)];

        // CHANCE AROUND POINT
        public static double RandomAroundPoint(double point, double variance)
        {
            double modifier = rng.NextDouble() * variance * 2 - variance;
            return point + variance;
        }

        // RANDOM WALK LIST OF BOUNDS
        public static HashSet<Vector2Int> RandomWalkBoundsList(List<Bounds> boundsList)
        {
            HashSet<Vector2Int> floorTiles = new HashSet<Vector2Int>();
            foreach (var bound in boundsList)
                floorTiles.UnionWith(RandomWalkBounds(bound, RandomAroundPoint(0.60, 0.15)));

            return floorTiles;
        }

        // RANDOM WALK WITHIN BOUNDS
        public static HashSet<Vector2Int> RandomWalkBounds(Bounds space, double minFill = 0.75, double jumpChance = 0.25)
        {
            HashSet<Vector2Int> floorTiles = new HashSet<Vector2Int>();

            // START IN CENTER OF SPACE
            var startPos = new Vector2Int(space.StartXY.X + space.Size.Width / 2, space.StartXY.Y + space.Size.Height / 2);

            var newPos = startPos;
            var prevPos = startPos;

            // CALCULATE TOTAL STEPS
            int steps = (int)Math.Floor(space.Size.Width * space.Size.Height * minFill);

            // CONTINUE UNTIL STEPS EXHAUSTED
            while (steps > 0)
            {
                if (!floorTiles.Contains(newPos)) steps--;
                floorTiles.Add(newPos);

                prevPos = newPos;
                do
                {
                    // RANDOM CHANCE TO JUMP TO ANY TILE IN LIST
                    if (rng.NextDouble() < jumpChance) newPos = floorTiles.ElementAt(rng.Next(0, floorTiles.Count));

                    newPos = prevPos;
                    newPos = newPos.Add(Rnd4Dir());
                } while (!InsideBounds(space, newPos));
            }

            // RETURN FLOOR TILES
            return floorTiles;
        }

        // INSIDE BOUNDS
        public static bool InsideBounds(Bounds space, Vector2Int pos) => pos.X >= space.StartXY.X && pos.Y >= space.StartXY.Y && pos.X <= space.EndXY.X && pos.Y <= space.EndXY.Y;

        // INSIDE BOUNDS 2
        public static bool InsideBounds2(Bounds space, Vector2Int pos) => pos.X > space.StartXY.X && pos.Y > space.StartXY.Y && pos.X < space.EndXY.X && pos.Y < space.EndXY.Y;
    }
}
