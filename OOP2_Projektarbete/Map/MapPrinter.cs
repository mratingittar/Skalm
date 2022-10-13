using Skalm.Actors;
using Skalm.Actors.Tile;
using Skalm.Display;
using Skalm.Grid;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Map
{
    internal class MapPrinter
    {
        MapManager mapManager;
        DisplayManager displayManager;
        IPrinter printer;
        Grid2D<BaseTile> tileGrid;

        private Queue<Vector2Int> positionsToUpdate;

        // CONSTRUCTOR I
        public MapPrinter(MapManager mapManager, DisplayManager displayManager)
        {
            this.mapManager = mapManager;
            this.displayManager = displayManager;

            positionsToUpdate = new Queue<Vector2Int>();

            tileGrid = mapManager.tileGrid;
            printer = displayManager.printer;
        }

        // METHOD CASH UPDATED TILES
        public void CacheUpdatedTile(Vector2Int oldPos, Vector2Int newPos)
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
                DrawSingleTile(tileGrid, tempPos.X, tempPos.Y);
            }
        }

        // REDRAW WHOLE MAP
        public void RedrawMap()
        {
            // LOOP THROUGH GRID COLUMNS & ROWS
            for (int j = 0; j < tileGrid.gridHeight; j++)
            {
                for (int i = 0; i < tileGrid.gridWidth; i++)
                {
                    //BaseTile tileCurr = tileGrid.GetGridObject(i, j)!;
                    //if (tileCurr.actorsAtPosition.Count > 0)
                    //    DrawSingleTile(tileCurr.actorsAtPosition[0]);
                    //else
                        DrawSingleTile(tileGrid, i, j);
                }
            }

            // DRAW ACTORS
            foreach (var actor in mapManager.gameObjects)
            {
                DrawSingleTile(actor.tile);
            }
        }

        // PRINT SINGLE TILE
        private void DrawSingleTile(Grid2D<BaseTile> tileGrid, int x, int y)
        {
            // GET TILE AT POSITION & TILE INFO
            BaseTile tileCurr = tileGrid.GetGridObject(x, y)!;
            char toPrint = tileCurr == null ? ' ' : tileCurr!.sprite;
            ConsoleColor printCol = tileCurr == null ? ConsoleColor.Gray : tileCurr.color;

            // PRINT TO CONSOLE
            Console.ForegroundColor = printCol;
            foreach (var tile in tileGrid.GetPlanePositions(x, y))
            {
                printer.PrintAtPosition(toPrint, tile.Y, tile.X);
            }

            // RESET CONSOLE PRINT COLOR
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        // PRINT SINGLE TILE OVERLOAD
        private void DrawSingleTile(ActorTile tileCurr)
        {
            // GET TILE AT POSITION & TILE INFO
            char toPrint = tileCurr == null ? ' ' : tileCurr!.sprite;
            ConsoleColor printCol = tileCurr == null ? ConsoleColor.Gray : tileCurr.color;

            // PRINT TO CONSOLE
            Console.ForegroundColor = printCol;
            foreach (var tile in tileGrid.GetPlanePositions(tileCurr!.posXY.X, tileCurr!.posXY.Y))
            {
                printer.PrintAtPosition(toPrint, tile.Y, tile.X);
            }

            // RESET CONSOLE PRINT COLOR
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
