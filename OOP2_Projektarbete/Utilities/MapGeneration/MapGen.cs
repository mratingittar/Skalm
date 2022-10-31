using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Utilities.MapGeneration
{
    internal static class MapGen
    {
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

        // DOOR CLEANER
        public static HashSet<Vector2Int> DoorCleaner(HashSet<Vector2Int> doorTiles, HashSet<Vector2Int> floorTiles)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>(doorTiles);
            foreach (var door in doorTiles)
            {
                if ((BSPgen.CheckNeighbors4Way(door, floorTiles) != 2) || (BSPgen.CheckNeighbors4Way(door, result) != 0))
                    result.Remove(door);
            }

            return result;
        }
    }
}
