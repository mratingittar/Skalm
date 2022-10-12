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
            char toPrint;

            for (int j = 0; j < mapManager.tileGrid.gridHeight; j++)
            {
                for (int i = 0; i < mapManager.tileGrid.gridWidth; i++)
                {
                    BaseTile tileCurr = mapManager.tileGrid.GetGridObject(i, j)!;
                    toPrint = tileCurr == null ? ' ' : tileCurr!.sprite;

                    foreach (var item in mapManager.tileGrid.GetPlanePositions(i, j))
                    {
                        displayManager.printer.PrintAtPosition(toPrint, item.Y, item.X);
                    }
                }
            }
        }
    }
}
