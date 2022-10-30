using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Utilities.MapGeneration
{
    internal static class BSPgen
    {
        static Random rng = new Random();

        // BINARY SPACE PARTITIONING
        public static List<Bounds> BSPgeneration(Bounds splitSpace, int minW, int minH)
        {
            Queue<Bounds> roomQueue = new Queue<Bounds>();
            List<Bounds> roomList = new List<Bounds>();
            roomQueue.Enqueue(splitSpace);

            // LOOP WHILE ROOMS CAN STILL BE SPLIT
            while (roomQueue.Count > 0)
            {
                var room = roomQueue.Dequeue();
                if ((room.Size.Height >= minH) && (room.Size.Width >= minW))
                {
                    if (rng.NextDouble() < 0.5f)
                    {
                        if (room.Size.Height >= minH * 2)
                        {
                            SplitHorizontally(minH, roomQueue, room);
                        }
                        else if (room.Size.Width >= minW * 2)
                        {
                            SplitVertically(minW, roomQueue, room);
                        }
                        else if (room.Size.Width >= minW && room.Size.Height >= minH)
                        {
                            roomList.Add(room);
                        }
                    }
                    else
                    {
                        if (room.Size.Width >= minW * 2)
                        {
                            SplitVertically(minW, roomQueue, room);
                        }
                        else if (room.Size.Height >= minH * 2)
                        {
                            SplitHorizontally(minH, roomQueue, room);
                        }
                        else if (room.Size.Width >= minW && room.Size.Height >= minH)
                        {
                            roomList.Add(room);
                        }
                    }
                }
            }

            // RETURN ROOM LIST
            return roomList;
        }

        // SPLIT SPACE VERTICAL
        private static void SplitVertically(int minW, Queue<Bounds> roomQueue, Bounds room, bool padding = true)
        {
            int splitPad = padding ? (int)Math.Ceiling(room.Size.Width / 8f) : 0;

            var WidthSplit = rng.Next(1 + splitPad, room.Size.Width + 1 - splitPad);

            var room1start = new Vector2Int(room.StartXY.X, room.StartXY.Y);
            var room1end = new Vector2Int(room.StartXY.X + WidthSplit, room.EndXY.Y);

            var room2start = new Vector2Int(room.StartXY.X + WidthSplit, room.StartXY.Y);
            var room2end = new Vector2Int(room.EndXY.X, room.EndXY.Y);

            Bounds room1 = new Bounds(room1start, room1end);
            Bounds room2 = new Bounds(room2start, room2end);

            roomQueue.Enqueue(room1);
            roomQueue.Enqueue(room2);
        }

        // SPLIT SPACE HORIZONTALLY
        private static void SplitHorizontally(int minH, Queue<Bounds> roomQueue, Bounds room, bool padding = true)
        {
            int splitPad = padding ? (int)Math.Ceiling(room.Size.Height / 8f) : 0;

            var HeightSplit = rng.Next(1 + splitPad, room.Size.Height + 1 - splitPad);

            var room1start = new Vector2Int(room.StartXY.X, room.StartXY.Y);
            var room1end = new Vector2Int(room.EndXY.X, room.StartXY.Y + HeightSplit);

            var room2start = new Vector2Int(room.StartXY.X, room.StartXY.Y + HeightSplit);
            var room2end = new Vector2Int(room.EndXY.X, room.EndXY.Y);

            Bounds room1 = new Bounds(room1start, room1end);
            Bounds room2 = new Bounds(room2start, room2end);

            roomQueue.Enqueue(room1);
            roomQueue.Enqueue(room2);
        }

        // ADD PADDING TO BOUNDS LIST
        public static List<Bounds> AddPaddingToBoundsList(List<Bounds> inList, int padding = 1)
        {
            List<Bounds> outList = new List<Bounds>();
            foreach (var bound in inList)
            {
                var startXY = new Vector2Int(bound.StartXY.X + padding, bound.StartXY.Y + padding);
                var endXY = new Vector2Int(bound.EndXY.X - padding, bound.EndXY.Y - padding);

                outList.Add(new Bounds(startXY, endXY));
            }

            return outList;
        }

        // FIND ROOM CENTER POINTS
        public static List<Vector2Int> FindRoomCenters(List<Bounds> inList)
        {
            List<Vector2Int> roomCenters = new List<Vector2Int>();
            foreach (var bounds in inList)
            {
                roomCenters.Add(new Vector2Int(bounds.StartXY.X + bounds.Size.Width / 2, bounds.StartXY.Y + bounds.Size.Height / 2));
            }

            return roomCenters;
        }

        // FIND BIGGEST ROOM
        public static int FindBiggestRoom(List<Bounds> boundsList)
        {
            int area = 0;
            int tempArea;
            int indeWidth = 0;
            for (int i = 0; i < boundsList.Count; i++)
            {
                tempArea = boundsList[i].Size.Width * boundsList[i].Size.Height;
                if (tempArea > area)
                {
                    area = tempArea;
                    indeWidth = i;
                }
            }

            return indeWidth;
        }

        // CORRIDOR MAKER
        public static (HashSet<Vector2Int>, HashSet<Vector2Int>) CorridorMaker(HashSet<Vector2Int> floorTiles, Vector2Int startPos, Vector2Int endPos)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();
            HashSet<Vector2Int> doors = new HashSet<Vector2Int>();

            // DETERMINE STEPS IN EACH DIRECTION
            int stepsX = Math.Abs(startPos.X - endPos.X);
            int stepsY = Math.Abs(startPos.Y - endPos.Y);

            Vector2Int moveDir = Vector2Int.Zero;

            // DETERMINE STARTING MOVE DIRECTION
            bool startVert = stepsY > stepsX;
            if (startVert)
            {
                if (startPos.Y - endPos.Y > 0)
                    moveDir = Vector2Int.Up;
                else
                    moveDir = Vector2Int.Down;
            }
            else
            {
                if (startPos.X - endPos.X > 0)
                    moveDir = Vector2Int.Left;
                else
                    moveDir = Vector2Int.Right;
            }

            var newPos = startPos;
            var prevPos = startPos;

            var counter = Math.Max(stepsY, stepsX);

            // CONTINUE WALKING WHILE NO STEP DIRECTION IS ZERO
            while (counter > 0)
            {
                prevPos = newPos;
                newPos = newPos.Add(moveDir);
                result.Add(newPos);
                if (!floorTiles.Contains(newPos) && (doors.Count <= 0))
                    doors.Add(newPos);

                counter--;
            }

            // CHANGE DIRECTION
            moveDir = Vector2Int.DirectionFromTo(newPos, endPos);
            counter = Math.Max(Math.Abs(newPos.X - endPos.X), Math.Abs(newPos.Y - endPos.Y));

            // WALK UNTIL STEPS EMPTY
            while (counter > 0)
            {
                prevPos = newPos;
                newPos = newPos.Add(moveDir);
                result.Add(newPos);
                if (floorTiles.Contains(newPos) && !floorTiles.Contains(prevPos))
                    doors.Add(prevPos);

                counter--;
            }

            // RETURN RESULTS
            return (result, doors);
        }

        // CONNECT ALL ROOMS
        public static (HashSet<Vector2Int>, HashSet<Vector2Int>) ConnectAllRooms(HashSet<Vector2Int> floorTiles, List<Vector2Int> roomPosList)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();
            HashSet<Vector2Int> doors = new HashSet<Vector2Int>();
            List<Vector2Int> roomList = roomPosList;

            Vector2Int currPos = roomList[0];

            // CONNECT WHILE ROOM CENTERS LEFT IN LIST
            while (roomList.Count > 1)
            {
                var closest = FindClosestVector(currPos, roomList);
                var tempResult = CorridorMaker(floorTiles, currPos, closest);
                result.UnionWith(tempResult.Item1);
                doors.UnionWith(tempResult.Item2);
                if (rng.NextDouble() < 0.5) roomList.Remove(currPos);
                currPos = closest;
            }

            // COMBINE WITH TOTAL FLOOR TILE SET
            result.UnionWith(floorTiles);

            return (result, doors);
        }

        // FIND CLOSEST VECTOR IN LIST
        public static Vector2Int FindClosestVector(Vector2Int origin, List<Vector2Int> posList)
        {
            (int, double) shortest = (0, 1000000);
            double temp;
            Vector2 v1 = new Vector2(origin.X, origin.Y);
            Vector2 v2;
            for (int i = 0; i < posList.Count; i++)
            {
                v2 = new Vector2(posList[i].X, posList[i].Y);
                temp = Vector2.Distance(v1, v2);
                if ((temp != 0) && (temp < shortest.Item2))
                    shortest = (i, temp);
            }

            return posList[shortest.Item1];
        }

        // CONVERT BOUNDS LIST TO HASHSET
        public static HashSet<Vector2Int> ConvertBoundsListBordersToHashSet(List<Bounds> inList)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();
            foreach (var bounds in inList)
            {
                for (int i = bounds.StartXY.X; i < bounds.EndXY.X; i++)
                {
                    result.Add(new Vector2Int(i, bounds.StartXY.Y));
                    result.Add(new Vector2Int(i, bounds.EndXY.Y));
                }

                for (int j = bounds.StartXY.Y; j < bounds.EndXY.Y; j++)
                {
                    result.Add(new Vector2Int(bounds.StartXY.X, j));
                    result.Add(new Vector2Int(bounds.EndXY.X, j));
                }
            }
            return result;
        }

        // CONVERT HASHSET TO ARRAY
        public static int[,] ConvertHashSetToArr(HashSet<Vector2Int> inList, int w, int h)
        {
            int[,] outArr = new int[w + 2, h + 2];

            foreach (var pos in inList)
            {
                outArr[pos.X, pos.Y] = 2;
            }

            return outArr;
        }
    }
}
