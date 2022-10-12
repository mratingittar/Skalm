using Skalm.Actors.Tile;
using Skalm.Display;
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

        public MapPrinter(MapManager mapManager, DisplayManager displayManager)
        {
            this.mapManager = mapManager;
            this.displayManager = displayManager;
        }

        // REDRAW WHOLE MAP
        public void RedrawMap()
        {
            BaseTile tileCurr;
            char toPrint;
            ConsoleColor printCol;

            // LOOP THROUGH GRID COLUMNS & ROWS
            for (int j = 0; j < mapManager.tileGrid.gridHeight; j++)
            {
                for (int i = 0; i < mapManager.tileGrid.gridWidth; i++)
                {
                    // GET TILE AT POSITION & TILE INFO
                    tileCurr = mapManager.tileGrid.GetGridObject(i, j)!;
                    toPrint = tileCurr == null ? ' ' : tileCurr!.sprite;
                    printCol = tileCurr == null ? ConsoleColor.White : tileCurr.color;

                    // PRINT TO CONSOLE
                    Console.ForegroundColor = printCol;
                    foreach (var item in mapManager.tileGrid.GetPlanePositions(i, j))
                    {
                        displayManager.printer.PrintAtPosition(toPrint, item.Y, item.X);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
