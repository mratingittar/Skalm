using Skalm;
using Skalm.Map;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Display
{
    internal class DisplayManagerGameWindow : IDisplayManager
    {
        public Bounds displayBounds { get; set; }
        public MapManager mapManager { get; set; }
        private Queue<Vector2Int> positionsToUpdate;

        // CONSTRUCTOR I
        public DisplayManagerGameWindow(Bounds displayBounds, MapManager mapManager)
        {
            this.displayBounds = displayBounds;
            this.mapManager = mapManager;
            positionsToUpdate = new Queue<Vector2Int>();
            DrawField();
        }

        // METHOD CASH UPDATED TILES
        private void CacheUpdatedTiles(Vector2Int oldPos, Vector2Int newPos)
        {
            positionsToUpdate.Enqueue(oldPos);
            positionsToUpdate.Enqueue(newPos);
        }

        // METHOD REDRAW CASHED TILES
        public void RedrawCachedTiles()
        {
            Vector2Int tempPos;
            while (positionsToUpdate.Count > 0)
            {
                tempPos = positionsToUpdate.Dequeue();
                DrawSingleTile(mapManager.mapArr, tempPos.X, tempPos.Y);
            }
        }

        // METHOD DRAW SINGLE TILE
        private void DrawSingleTile(int[,] mapArr, int x, int y)
        {
            // DECIDE TILE TO DRAW
            switch (mapArr[x, y])
            {
                // FLOOR
                case (int)MapTiles.Floor:
                    Console.Write(Globals.G_FLOOR);
                    break;

                // WALL
                case (int)MapTiles.Wall:
                    Console.Write(Globals.G_WALL);
                    break;

                // DOOR
                case (int)MapTiles.Door:
                    Console.Write(Globals.G_DOOR);
                    break;

                // VOID
                case (int)MapTiles.Void:
                    Console.Write(' ');
                    break;

                // DEFAULT : EMPTY
                default:
                    Console.Write(' ');
                    break;
            }

            // IF DRAW SQUARE SYMBOLS IN GAME VIEW, DRAW EXTRA SPACE
            if (Globals.G_HALFWIDTHDRAW) Console.Write(' ');
        }

        // METHOD DRAW FIELD
        public void DrawField()
        {
            // VERTICAL AXIS
            for (int j = 0; j < mapManager.mapArr.GetLength(1); j++)
            {
                // SET CURSOR POSITION
                Console.SetCursorPosition(displayBounds.StartXY.X, displayBounds.StartXY.Y + j);

                // HORIZONTAL AXIS
                for (int i = 0; i < mapManager.mapArr.GetLength(0); i++)
                {
                    DrawSingleTile(mapManager.mapArr, i, j);
                }

                // NEXT LINE
                Console.WriteLine();
            }
        }
    }
}
