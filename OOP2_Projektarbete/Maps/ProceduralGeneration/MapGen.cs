using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Maps.ProceduralGeneration
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
                if (BSPgen.CheckNeighbors4Way(door, floorTiles) != 2 || BSPgen.CheckNeighbors4Way(door, result) != 0)
                    result.Remove(door);
            }

            return result;
        }

        // FILL AROUND POINT
        public static HashSet<Vector2Int> FillAroundPosition(Vector2Int pos, int steps = 1)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();

            for (int j = pos.Y - steps; j <= pos.Y + steps; j++)
            {
                for (int i = pos.X - steps; i <= pos.X + steps; i++)
                {
                    result.Add(new Vector2Int(i, j));
                }
            }

            return result;
        }

        // FIND FURTHEST VECTOR IN LIST
        public static Vector2Int FindFurthestVectorInList(Vector2Int origin, List<Vector2Int> posList)
        {
            (int, double) furthest = (0, 0);
            double temp;
            Vector2 v1 = new Vector2(origin.X, origin.Y);
            Vector2 v2;
            for (int i = 0; i < posList.Count; i++)
            {
                v2 = new Vector2(posList[i].X, posList[i].Y);
                temp = Vector2.Distance(v1, v2);
                if (temp != 0 && temp > furthest.Item2)
                    furthest = (i, temp);
            }

            return posList[furthest.Item1];
        }
    }
}
