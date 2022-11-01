using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Maps.ProceduralGeneration
{
    internal static class RoomGen
    {
        static Random rng = new Random();

        // GET RANDOM DIR
        private static Vector2Int GetRandomDir() => new Vector2Int(rng.Next(0, 3) - 1, rng.Next(0, 3) - 1);

        // CREATE RANDOM ROOMS FROM BOUNDS LIST
        public static HashSet<Vector2Int> CreateRandomRoomsFromList(List<Bounds> boundList, double rwChance = 0.75, double fillRate = 0.65)
        {
            HashSet<Vector2Int> roomTiles = new HashSet<Vector2Int>();

            foreach (var bound in boundList)
                roomTiles.UnionWith(CreateRandomRoom(bound, rwChance, 0.6 + rng.NextDouble() * 0.2));

            return roomTiles;
        }

        // CREATE RANDOM ROOM AT BOUNDS
        public static HashSet<Vector2Int> CreateRandomRoom(Bounds space, double rwChance = 0.75, double fillRate = 0.65)
        {
            if (rng.NextDouble() < rwChance)
                return RWgen.RandomWalkBounds(space, fillRate);
            else
                return CreateNormalRoom(space);
        }

        // CREATE NORMAL ROOM FROM BOUDNS
        public static HashSet<Vector2Int> CreateNormalRoom(Bounds space)
        {
            HashSet<Vector2Int> floorTiles = new HashSet<Vector2Int>();

            int startX1 = space.StartXY.X + rng.Next(0, 2);
            int startY1 = space.StartXY.Y + rng.Next(0, 2);
            int endX2 = space.EndXY.X - rng.Next(0, 2);
            int endY2 = space.EndXY.Y - rng.Next(0, 2);

            bool noCorners = rng.NextDouble() < 0.5;
            bool pillars = rng.NextDouble() < 0.5;

            // FILL WHOLE ROOM SPACE
            for (int j = startY1; j <= endY2; j++)
            {
                for (int i = startX1; i <= endX2; i++)
                {
                    // ADD TILE
                    floorTiles.Add(new Vector2Int(i, j));
                }
            }

            // NO CORNERS
            if (noCorners)
            {
                floorTiles.Remove(new Vector2Int(startX1, startY1));
                floorTiles.Remove(new Vector2Int(endX2, startY1));
                floorTiles.Remove(new Vector2Int(startX1, endY2));
                floorTiles.Remove(new Vector2Int(endX2, endY2));
            }

            // PILLARS
            if (pillars)
            {
                floorTiles.Remove(new Vector2Int(startX1 + 2, startY1 + 2));
                floorTiles.Remove(new Vector2Int(endX2 - 2, startY1 + 2));
                floorTiles.Remove(new Vector2Int(startX1 + 2, endY2 - 2));
                floorTiles.Remove(new Vector2Int(endX2 - 2, endY2 - 2));
            }

            return floorTiles;
        }

        // REMOVE RANDOM ROOMS UNTIL MAX
        public static List<Bounds> MaximumRoomsList(List<Bounds> boundList, int maxRooms = 8)
        {
            while (boundList.Count > maxRooms)
                boundList.RemoveAt(rng.Next(0, boundList.Count));

            return boundList;
        }

        // FILL AROUND ROOM CENTERS
        public static HashSet<Vector2Int> FillAroundRoomCenters(IEnumerable<Vector2Int> roomCenters, int steps = 1)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();
            foreach (var pos in roomCenters)
                result.UnionWith(MapGen.FillAroundPosition(pos, steps));

            return result;
        }

        // REMOVE UNUSED ROOM CENTERS
        public static List<Vector2Int> RemoveEmptyRoomCenters(List<Bounds> boundsList, List<Vector2Int> roomList)
        {
            List<Vector2Int> result = new List<Vector2Int>(roomList);
            bool isInsideBounds = false;

            // LOOP THROUGH ROOM CENTERS
            foreach (var pos in roomList)
            {
                // LOOP THROUGH BOUNDS LIST
                isInsideBounds = false;
                foreach (var bounds in boundsList)
                {
                    if (RWgen.InsideBounds(bounds, pos))
                    {
                        isInsideBounds = true;
                        break;
                    }
                }

                // IF INSIDE NO BOUNDS, REMOVE FROM LIST
                if (!isInsideBounds)
                    result.Remove(pos);
            }

            return result;
        }

        // FIND WALLS
        public static HashSet<Vector2Int> FindAllWalls(HashSet<Vector2Int> floorTiles)
        {
            HashSet<Vector2Int> walls = new HashSet<Vector2Int>();
            Vector2Int v;
            for (int j = 1; j < floorTiles.Count - 1; j++)
            {
                for (int i = 1; i < floorTiles.Count; i++)
                {
                    v = new Vector2Int(i, j);
                    if (!floorTiles.Contains(v) && BSPgen.CheckNeighbors8Way(v, floorTiles) > 0)
                        walls.Add(v);
                }
            }

            return walls;
        }

        // DRAW ALL POSITIONS IN COLLECTION
        public static void DrawAllPositionsInCollection(IEnumerable<Vector2Int> doorList, char sprite, ConsoleColor color = ConsoleColor.Yellow)
        {
            foreach (var pos in doorList)
            {
                Console.SetCursorPosition(pos.X, pos.Y);
                Console.ForegroundColor = color;
                Console.Write(sprite);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        // DRAW SINGLE TILE
        public static void DrawSingleTile(Vector2Int position, char sprite)
        {
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(sprite);
        }
    }
}
