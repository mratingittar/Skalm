using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Maps.ProceduralGeneration
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
                if (room.Size.Height >= minH && room.Size.Width >= minW)
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
            int splitPad = padding ? (int)Math.Ceiling(room.Size.Width / 7f) : 0;

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
            int splitPad = padding ? (int)Math.Ceiling(room.Size.Height / 7f) : 0;

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

        // ADD RANDOM PADDING TO BOUNDS LIST
        public static List<Bounds> AddRandomPaddingToBoundsList(List<Bounds> inList, int padding = 2)
        {
            List<Bounds> outList = new List<Bounds>();
            int paddingX1, paddingY1, paddingX2, paddingY2;
            foreach (var bound in inList)
            {
                paddingX1 = rng.Next(0, padding);
                paddingY1 = rng.Next(0, padding);
                paddingX2 = rng.Next(0, padding);
                paddingY2 = rng.Next(0, padding);

                var startXY = new Vector2Int(bound.StartXY.X + paddingX1, bound.StartXY.Y + paddingY1);
                var endXY = new Vector2Int(bound.EndXY.X - paddingX2, bound.EndXY.Y - paddingY2);

                outList.Add(new Bounds(startXY, endXY));
            }

            return outList;
        }

        // CONNECT ALL ROOMS
        public static (HashSet<Vector2Int>, HashSet<Vector2Int>) ConnectAllRooms(HashSet<Vector2Int> floorTiles, List<Vector2Int> roomPosList, List<Bounds> boundsList)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();
            HashSet<Vector2Int> doors = new HashSet<Vector2Int>();
            List<Vector2Int> roomList = new List<Vector2Int>();

            // COPY ROOM LIST
            foreach (var pos in roomPosList)
                roomList.Add(pos);

            Vector2Int currPos = roomList[0];
            double dist;

            // CONNECT WHILE ROOM CENTERS LEFT IN LIST
            while (roomList.Count > 1)
            {
                // FIND CLOSEST ROOM CENTER & MAKE CONNECTION
                var closest = FindClosestVector(currPos, roomList);
                dist = closest.Item2;
                //var tempResult = CorridorMaker(floorTiles, currPos, closest.Item1);
                var tempResult = CorridorMakerFindDoorsBounds(floorTiles, currPos, closest.Item1, boundsList);
                result.UnionWith(tempResult.Item1);
                doors.UnionWith(tempResult.Item2);

                roomList.Remove(closest.Item1);

                // MAKE MORE CONNECTIONS?
                var closest2 = FindClosestVector(currPos, roomList);
                if (closest2.Item2 < dist * 1.75)
                {
                    //tempResult = CorridorMaker(floorTiles, currPos, closest2.Item1);
                    tempResult = CorridorMakerFindDoorsBounds(floorTiles, currPos, closest2.Item1, boundsList);
                    result.UnionWith(tempResult.Item1);
                    doors.UnionWith(tempResult.Item2);
                }

                roomList.Add(closest.Item1);
                roomList.Remove(currPos);

                // SET START POSITION TO PREVIOUS TARGET
                currPos = closest.Item1;
            }

            // COMBINE WITH TOTAL FLOOR TILE SET
            result.UnionWith(floorTiles);

            return (result, doors);
        }

        #region Finders (Rooms, Closest Vectors, Doors, etc)

        // FIND CLOSEST VECTOR IN LIST
        public static (Vector2Int, double) FindClosestVector(Vector2Int origin, List<Vector2Int> posList)
        {
            (int, double) shortest = (0, 1000000);
            double temp;
            Vector2 v1 = new Vector2(origin.X, origin.Y);
            Vector2 v2;
            for (int i = 0; i < posList.Count; i++)
            {
                v2 = new Vector2(posList[i].X, posList[i].Y);
                temp = Vector2.Distance(v1, v2);
                if (temp != 0 && temp < shortest.Item2)
                    shortest = (i, temp);
            }

            return (posList[shortest.Item1], shortest.Item2);
        }

        // FIND ROOM CENTER POINTS
        public static List<Vector2Int> FindRoomCenters(List<Bounds> inList)
        {
            List<Vector2Int> roomCenters = new List<Vector2Int>();
            foreach (var bounds in inList)
                roomCenters.Add(new Vector2Int(bounds.StartXY.X + bounds.Size.Width / 2, bounds.StartXY.Y + bounds.Size.Height / 2));

            return roomCenters;
        }

        // FIND DOORS FROM BOUNDS LIST
        public static HashSet<Vector2Int> FindDoorsFromBoundsList(List<Bounds> roomList, HashSet<Vector2Int> floorTiles)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();
            Vector2Int tempPos = Vector2Int.Zero;
            int doorCounter = 0;

            // LOOP THROUGH ALL BOUNDS
            foreach (var bounds in roomList)
            {
                // HORIZONTAL EDGES
                for (int i = bounds.StartXY.X; i < bounds.EndXY.X; i++)
                {
                    tempPos = new Vector2Int(i, bounds.StartXY.Y);
                    if (floorTiles.Contains(tempPos)
                    && CheckNeighbors4Way(tempPos, floorTiles) <= 2
                    && CheckNeighbors4Way(tempPos, result) <= 0)
                        result.Add(tempPos);

                    tempPos = new Vector2Int(i, bounds.EndXY.Y);
                    if (floorTiles.Contains(tempPos)
                    && CheckNeighbors4Way(tempPos, floorTiles) <= 2
                    && CheckNeighbors4Way(tempPos, result) <= 0)
                        result.Add(tempPos);
                }

                // VERTICAL EDGES
                for (int j = bounds.StartXY.Y; j < bounds.EndXY.Y; j++)
                {
                    tempPos = new Vector2Int(bounds.StartXY.X, j);
                    if (floorTiles.Contains(tempPos)
                    && CheckNeighbors4Way(tempPos, floorTiles) <= 2
                    && CheckNeighbors4Way(tempPos, result) <= 0)
                        result.Add(tempPos);

                    tempPos = new Vector2Int(bounds.EndXY.X, j);
                    if (floorTiles.Contains(tempPos)
                    && CheckNeighbors4Way(tempPos, floorTiles) <= 2
                    && CheckNeighbors4Way(tempPos, result) <= 0)
                        result.Add(tempPos);
                }
            }

            // RETURN DOOR POSITIONS
            return result;
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

        #endregion

        #region Corridor Makers

        // CORRIDOR MAKER
        public static (HashSet<Vector2Int>, HashSet<Vector2Int>) CorridorMaker(HashSet<Vector2Int> floorTiles, Vector2Int startPos, Vector2Int endPos)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();
            HashSet<Vector2Int> doors = new HashSet<Vector2Int>();

            result.Add(startPos);

            // DETERMINE STEPS IN EACH DIRECTION
            int stepsX = Math.Abs(startPos.X - endPos.X);
            int stepsY = Math.Abs(startPos.Y - endPos.Y);
            var counter = Math.Max(stepsY, stepsX);

            // FIND DIRECTION
            Vector2Int moveDir = FindDirection4Way(startPos, endPos);

            var newPos = startPos;

            // MAKE CORRIDOR IN CHOSEN DIRECTION OF SPECIFIED LENGTH
            var corridor = CorridorInDir(newPos, moveDir, counter);

            newPos = corridor.Item1;
            result.UnionWith(corridor.Item2);

            // CHANGE DIRECTION
            moveDir = Vector2Int.DirectionFromTo(newPos, endPos);
            counter = Math.Max(Math.Abs(newPos.X - endPos.X), Math.Abs(newPos.Y - endPos.Y));

            // MAKE CORRIDOR IN CHOSEN DIRECTION OF SPECIFIED LENGTH
            corridor = CorridorInDir(newPos, moveDir, counter);

            result.UnionWith(corridor.Item2);

            // RETURN (CORRIDOR TILES & DOOR TILES)
            return (result, doors);
        }

        // CORRIDOR MAKER FIND DOORS FROM BOUNDS LIST
        public static (HashSet<Vector2Int>, HashSet<Vector2Int>) CorridorMakerFindDoorsBounds(HashSet<Vector2Int> floorTiles, Vector2Int startPos, Vector2Int endPos, List<Bounds> boundsList)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();
            HashSet<Vector2Int> doors = new HashSet<Vector2Int>();

            result.Add(startPos);

            // DETERMINE STEPS IN EACH DIRECTION
            int stepsX = Math.Abs(startPos.X - endPos.X);
            int stepsY = Math.Abs(startPos.Y - endPos.Y);
            var counter = Math.Max(stepsY, stepsX);

            // FIND DIRECTION
            Vector2Int moveDir = FindDirection4Way(startPos, endPos);

            var newPos = startPos;

            // CONTINUE WALKING WHILE NO STEP DIRECTION IS ZERO
            var corridor = CorridorInDirFindDoors(newPos, moveDir, counter, floorTiles, boundsList);

            newPos = corridor.Item1;
            result.UnionWith(corridor.Item2);
            doors.UnionWith(corridor.Item3);

            // CHANGE DIRECTION
            moveDir = Vector2Int.DirectionFromTo(newPos, endPos);
            counter = Math.Max(Math.Abs(newPos.X - endPos.X), Math.Abs(newPos.Y - endPos.Y));

            // WALK UNTIL STEPS EMPTY
            corridor = CorridorInDirFindDoors(newPos, moveDir, counter, floorTiles, boundsList);

            result.UnionWith(corridor.Item2);
            doors.UnionWith(corridor.Item3);

            // RETURN (CORRIDOR TILES & DOOR TILES)
            return (result, doors);
        }

        // CORRIDOR IN DIRECTION
        public static (Vector2Int, HashSet<Vector2Int>) CorridorInDir(Vector2Int start, Vector2Int dir, int steps)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();

            // ADD START POSITION
            Vector2Int newPos = start;

            result.Add(newPos);

            // CONTINUE WALKING WHILE NO STEP DIRECTION IS ZERO
            while (steps > 0)
            {
                newPos = newPos.Add(dir);
                result.Add(newPos);

                steps--;
            }

            // RETURNS (LAST POSITION & CORRIDOR TILES)
            return (newPos, result);
        }

        // CORRIDOR IN DIRECTION FIND DOORS
        public static (Vector2Int, HashSet<Vector2Int>, HashSet<Vector2Int>) CorridorInDirFindDoors(Vector2Int start, Vector2Int dir, int steps, HashSet<Vector2Int> floorTiles, List<Bounds> boundsList)
        {
            HashSet<Vector2Int> corrTiles = new HashSet<Vector2Int>();
            HashSet<Vector2Int> doorTiles = new HashSet<Vector2Int>();
            List<Bounds> visitedRooms = new List<Bounds>(boundsList);
            Vector2Int newPos = start;

            int doorCount = 0;

            corrTiles.Add(newPos);

            // CONTINUE WALKING WHILE NO STEP DIRECTION IS ZERO
            while (steps > 0)
            {
                // NEXT STEP IN DIRECTION
                newPos = newPos.Add(dir);
                corrTiles.Add(newPos);

                steps--;

                // CHECK IF COORD CAN BE DOOR
                var doorCheck = CoordinateOnBoundsEdgeList(boundsList, newPos);
                if (doorCheck.Item1
                    && doorCount < 2
                    && CheckNeighbors4Way(newPos, doorTiles) == 0
                    && visitedRooms.Contains(doorCheck.Item2))
                {
                    doorTiles.Add(newPos);
                    visitedRooms.Remove(doorCheck.Item2);
                    doorCount++;
                }
            }

            // RETURN (CORRIDOR TILES & DOOR TILES)
            return (newPos, corrTiles, doorTiles);
        }

        #endregion

        #region Bounds Checks

        // CHECK IF COORDINATE ON ANY BOUNDS EDGE IN LIST
        public static (bool, Bounds) CoordinateOnBoundsEdgeList(IEnumerable<Bounds> boundsList, Vector2Int pos)
        {
            foreach (var bounds in boundsList)
                if (CoordinateOnBoundsEdge(bounds, pos)) return (true, bounds);

            return (false, new Bounds());
        }

        // CHECK IF COORDINATE IS ON BOUNDS EDGE
        public static bool CoordinateOnBoundsEdge(Bounds bounds, Vector2Int pos)
        {
            // HORIZONTAL EDGE
            for (int i = bounds.StartXY.X; i <= bounds.EndXY.X; i++)
            {
                if (pos.X == i && pos.Y == bounds.StartXY.Y) return true;
                if (pos.X == i && pos.Y == bounds.EndXY.Y) return true;
            }

            // VERTICAL EDGE
            for (int j = bounds.StartXY.Y; j <= bounds.EndXY.Y; j++)
            {
                if (pos.X == bounds.StartXY.X && pos.Y == j) return true;
                if (pos.X == bounds.EndXY.X && pos.Y == j) return true;
            }

            return false;
        }

        #endregion

        #region Directions & Neighbors

        // FIND DIRECTION 4-WAY
        public static Vector2Int FindDirection4Way(Vector2Int start, Vector2Int end)
        {
            // DETERMINE STEPS IN EACH DIRECTION
            int stepsX = Math.Abs(start.X - end.X);
            int stepsY = Math.Abs(start.Y - end.Y);

            Vector2Int moveDir = Vector2Int.Zero;

            // DETERMINE STARTING MOVE DIRECTION
            bool startVert = stepsY > stepsX;
            if (startVert)
            {
                if (start.Y - end.Y > 0)
                    moveDir = Vector2Int.Up;
                else
                    moveDir = Vector2Int.Down;
            }
            else
            {
                if (start.X - end.X > 0)
                    moveDir = Vector2Int.Left;
                else
                    moveDir = Vector2Int.Right;
            }

            return moveDir;
        }

        // CHECK NEIGHBORS 4-WAY
        public static int CheckNeighbors4Way(Vector2Int pos, HashSet<Vector2Int> tiles)
        {
            int counter = 0;

            if (tiles.Contains(pos.Add(Vector2Int.Up))) counter++;
            if (tiles.Contains(pos.Add(Vector2Int.Down))) counter++;
            if (tiles.Contains(pos.Add(Vector2Int.Right))) counter++;
            if (tiles.Contains(pos.Add(Vector2Int.Left))) counter++;

            return counter;
        }

        // CHECK NEIGHBORS 8-WAY
        public static int CheckNeighbors8Way(Vector2Int pos, HashSet<Vector2Int> tiles)
        {
            int counter = 0;

            if (tiles.Contains(pos)) counter++;

            if (tiles.Contains(pos.Add(Vector2Int.Up))) counter++;
            if (tiles.Contains(pos.Add(Vector2Int.Down))) counter++;
            if (tiles.Contains(pos.Add(Vector2Int.Right))) counter++;
            if (tiles.Contains(pos.Add(Vector2Int.Left))) counter++;

            if (tiles.Contains(pos.Add(Vector2Int.Left.Add(Vector2Int.Up)))) counter++;
            if (tiles.Contains(pos.Add(Vector2Int.Right.Add(Vector2Int.Up)))) counter++;
            if (tiles.Contains(pos.Add(Vector2Int.Left.Add(Vector2Int.Down)))) counter++;
            if (tiles.Contains(pos.Add(Vector2Int.Right.Add(Vector2Int.Down)))) counter++;

            return counter;
        }

        #endregion

        #region Collection Converters

        // CONVERT BOUNDS LIST TO HASHSET
        public static HashSet<Vector2Int> ConvertBoundsListBordersToHashSet(List<Bounds> inList)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();
            foreach (var bounds in inList)
            {
                for (int i = bounds.StartXY.X; i <= bounds.EndXY.X; i++)
                {
                    result.Add(new Vector2Int(i, bounds.StartXY.Y));
                    result.Add(new Vector2Int(i, bounds.EndXY.Y));
                }

                for (int j = bounds.StartXY.Y; j <= bounds.EndXY.Y; j++)
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

        #endregion
    }
}
