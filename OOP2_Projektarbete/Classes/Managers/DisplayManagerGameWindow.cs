using OOP2_Projektarbete.Classes.Map;
using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Managers
{
    internal class DisplayManagerGameWindow : IDisplayManager
    {
        public Bounds2Int displayBounds { get; set; }
        public MapManager mapManager { get; set; }

        // CONSTRUCTOR I
        public DisplayManagerGameWindow(Bounds2Int displayBounds, MapManager mapManager)
        {
            this.displayBounds = displayBounds;
            this.mapManager = mapManager;
            DrawField();
        }

        // METHOD DRAW FIELD
        public void DrawField()
        {
            for (int j = 0; j < mapManager.mapArr.GetLength(1); j++)
            {
                Console.SetCursorPosition(displayBounds.startXY.X, displayBounds.startXY.Y + j);

                for (int i = 0; i < mapManager.mapArr.GetLength(0); i++)
                {
                    switch (mapManager.mapArr[i,j])
                    {
                        case (int)MapTiles.Floor:
                        Console.Write(Globals.G_FLOOR);
                        break;

                        case (int)MapTiles.Wall:
                        Console.Write(Globals.G_WALL);
                        break;

                        case (int)MapTiles.Void:
                        Console.Write(' ');
                        break;

                        case (int)MapTiles.Door:
                        Console.Write(Globals.G_DOOR);
                        break;

                        default:
                        Console.Write(' ');
                        break;
                    }

                    if (Globals.G_HALFWIDTHDRAW) Console.Write(' ');
                }

                Console.WriteLine();
            }
        }
    }
}
