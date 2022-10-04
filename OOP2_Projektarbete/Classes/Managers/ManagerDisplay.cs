using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Managers
{
    internal class ManagerDisplay
    {
        // METHOD INITIALIZE WINDOW
        public void InitGameWindow()
        {
            int gameW = Globals.G_GAME_WIDTH + (Globals.G_HUD_PADDING * 2);
            int messageW = gameW;
            int mainStatsW = Globals.G_HUD_MAINSTATS_W + (Globals.G_HUD_PADDING * 2);
            int subStatsW = Globals.G_HUD_SUBSTATS_W + (Globals.G_HUD_PADDING * 2);

            int WindowWidth = gameW + mainStatsW;

            int gameH = Globals.G_GAME_HEIGHT + (Globals.G_HUD_PADDING * 2);
            int messageH = Globals.G_HUD_MSGBOX_H + (Globals.G_HUD_PADDING * 2);
            int mainStatsH = Globals.G_HUD_MAINSTATS_H + (Globals.G_HUD_PADDING * 2);
            int subStatsH = Globals.G_HUD_SUBSTATS_H + (Globals.G_HUD_PADDING * 2);

            int WindowHeight = gameH + messageH;

            Console.SetWindowSize(WindowWidth+1, WindowHeight+1);
            Console.SetBufferSize(WindowWidth+1, WindowHeight+1);

            // PRINT GAME PLAN BORDERS
            BorderPrinter(new Vector2Int(0,0), new Vector2Int(gameW, gameH));

            // PRINT MESSAGE FIELD BORDERS
            BorderPrinter(new Vector2Int(0, gameH), new Vector2Int(messageW, gameH + messageH));

            // PRINT MAIN STATS BORDERS
            BorderPrinter(new Vector2Int(gameW, 0), new Vector2Int(gameW + mainStatsW, mainStatsH));

            // PRINT SUB STATS BORDERS
            BorderPrinter(new Vector2Int(gameW, mainStatsH), new Vector2Int(gameW + subStatsW, mainStatsH + subStatsH));
        }

        // METHOD BORDER PRINTER
        public void BorderPrinter(Vector2Int topleft, Vector2Int bottomright)
        {
            // PRINT BORDER HORIZONTAL AXIS
            for (int i = topleft.X; i < bottomright.X; i++)
            {
                PrintAtPosition(i, topleft.Y, Globals.G_BORDER);
                PrintAtPosition(i, bottomright.Y, Globals.G_BORDER);
            }

            // PRINT BORDER VERTICAL AXIS
            for (int j = topleft.Y; j < bottomright.Y; j++)
            {
                PrintAtPosition(topleft.X, j, Globals.G_BORDER);
                PrintAtPosition(bottomright.X, j, Globals.G_BORDER);
            }
        }

        // METHOD PRINT AT POSITION
        public void PrintAtPosition(int x, int y, char chr)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(chr);
        }
    }
}
