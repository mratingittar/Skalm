using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Utilities.MapGeneration
{
    internal static class RoomGen
    {
        static Random rng = new Random();

        // CREATE RANDOM ROOMS FROM BOUNDS LIST
        public static HashSet<Vector2Int> CreateRandomRoomsFromList(List<Bounds> boundList, double rwChance = 0.5, double fillRate = 0.65)
        {
            HashSet<Vector2Int> roomTiles = new HashSet<Vector2Int>();

            foreach (var bound in boundList)
                roomTiles.UnionWith(CreateRandomRoom(bound, rwChance, fillRate));

            return roomTiles;
        }

        // CREATE RANDOM ROOM AT BOUNDS
        public static HashSet<Vector2Int> CreateRandomRoom(Bounds space, double rwChance = 0.5, double fillRate = 0.65)
        {
            HashSet<Vector2Int> room = new HashSet<Vector2Int>();

            if (rng.NextDouble() < rwChance)
                return RWgen.RandomWalkBounds(space, fillRate);
            else
                return CreateNormalRoom(space);
        }

        // CREATE NORMAL ROOM FROM BOUDNS
        public static HashSet<Vector2Int> CreateNormalRoom(Bounds space)
        {
            HashSet<Vector2Int> floorTiles = new HashSet<Vector2Int>();

            // FILL WHOLE ROOM SPACE
            for (int j = space.StartXY.Y; j < space.EndXY.Y; j++)
            {
                for (int i = space.StartXY.X; i < space.EndXY.X; i++)
                {
                    floorTiles.Add(new Vector2Int(i, j));
                }
            }

            return floorTiles;
        }

        // REMOVE RANDOM ROOMS UNTIL MAX
        public static List<Bounds> MaximumRoomsList(List<Bounds> boundList, int maxRooms = 8)
        {
            while (boundList.Count > maxRooms)
            {
                int remove = rng.Next(0, boundList.Count);
                boundList.RemoveAt(remove);
            }

            return boundList;
        }
    }
}
