﻿using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Map
{
    internal static class MapGeneration
    {
        // CARDINAL DIRS ARRAY
        public static Vector2Int[] cardinalDirs = {
            new Vector2Int( 1,  0),
            new Vector2Int( 0,  1),
            new Vector2Int(-1,  0),
            new Vector2Int( 0, -1)
        };

        // METHOD GET CELL NEIGHBORS VON NEUMAN (4-WAY)
        public static int Cell4W(int[,] inArr, int x, int y, int value = 1)
        {
            int count = 0;
            foreach (var dir in cardinalDirs)
                if ((InsideArrBounds(inArr, x + dir.X, y + dir.Y)) && (inArr[x + dir.X, y + dir.Y] == value)) count++;
            //if ((InsideArrBounds(inArr, x, y)) && (inArr[x, y - 1] == value)) output++;
            //if ((InsideArrBounds(inArr, x, y)) && (inArr[x, y + 1] == value)) output++;
            //if ((InsideArrBounds(inArr, x, y)) && (inArr[x - 1, y] == value)) output++;
            //if ((InsideArrBounds(inArr, x, y)) && (inArr[x + 1, y] == value)) output++;
            return count;
        }

        // METHOD GET CELL NEIGHBORS 8-WAY
        public static int Cell8W(int[,] inArr, int x, int y, int value = 1)
        {
            int count = 0;
            for (int j = (y - 1); j <= (y + 1); j++)
            {
                for (int i = (x + 1); i <= (x + 1); i++)
                {
                    if ((InsideArrBounds(inArr, i, j)) && (inArr[x + i, y + j] == value)) count++;
                }
            }

            return count;
        }

        // METHOD CHECK IF COORDINATE INSIDE ARRAY
        public static bool InsideArrBounds(int[,] inArr, int x, int y)
        {
            if ((x < 0)
            || (x >= inArr.GetLength(0))
            || (y < 0)
            || (y >= inArr.GetLength(1)))
                return false;
            else
                return true;
        }
    }
}