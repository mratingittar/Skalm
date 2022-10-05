using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Map
{
    internal class MapManager
    {
        private int mapW;
        private int mapH;

        public int[,] mapArr { get; private set; }

        // CONSTRUCTOR I
        public MapManager()
        {
            mapW = Globals.G_GAME_WIDTH;
            mapH = Globals.G_GAME_HEIGHT;

            if (Globals.G_HALFWIDTHDRAW) mapW /= 2;

            mapArr = new int[mapW, mapH];
            InitMap();
        }

        // METHOD INITIALIZE MAP
        public void InitMap()
        {
            for (int j = 4; j < mapArr.GetLength(1)-4; j++)
            {
                for (int i = 4; i < mapArr.GetLength(0)-4; i++)
                {
                    mapArr[i, j] = (int)MapTiles.Floor;
                }
            }

            FindWalls();
        }

        // METHOD CELL VON NEUMAN
        private int cell4W(int x, int y, int value)
        {
            int output = 0;
            if (mapArr[x, y - 1] == value) output++;
            if (mapArr[x, y + 1] == value) output++;
            if (mapArr[x - 1, y] == value) output++;
            if (mapArr[x + 1, y] == value) output++;
            return output;
        }

        // FIND WALLS IN ARRAY
        private void FindWalls()
        {
            for (int j = 0; j < mapArr.GetLength(1); j++)
            {
                for (int i = 0; i < mapArr.GetLength(0); i++)
                {
                    if (mapArr[i, j] == (int)MapTiles.Floor)
                        if (cell4W(i, j, (int)MapTiles.Void) > 0) mapArr[i, j] = (int)MapTiles.Wall;
                }
            }
        }
    }

    public enum MapTiles
    {
        Void,
        Wall,
        Floor,
        Door
    }
}
