using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Managers
{
    internal class WindowManagerConsole : IWindowManager
    {
        // INSTANCE VARIABLES
        public Bounds gameWindowBounds { get; private set; }
        private Vector2Int gwStartXY;
        private Vector2Int gwEndXY;

        public Bounds msgBoxBounds { get; private set; }
        private Vector2Int msgBoxStartXY;
        private Vector2Int msgBoxEndXY;

        public Bounds mainStatsBounds { get; private set; }
        private Vector2Int mainStatsStartXY;
        private Vector2Int mainStatsEndXY;

        public Bounds subStatsBounds { get; private set; }
        private Vector2Int subStatsStartXY;
        private Vector2Int subStatsEndXY;

        // CONSTRUCTOR I
        public WindowManagerConsole()
        {
            InitHudPositions();
            InitGameWindow();
        }

        // SET WINDOW COORDS
        private void InitHudPositions()
        {
            gwStartXY = new Vector2Int(Globals.G_HUD_PADDING, Globals.G_HUD_PADDING);
            gwEndXY = new Vector2Int(gwStartXY.X + Globals.G_GAME_WIDTH, gwStartXY.Y + Globals.G_GAME_HEIGHT);
            gameWindowBounds = new Bounds(gwStartXY, gwEndXY);

            msgBoxStartXY = new Vector2Int(Globals.G_HUD_PADDING, gwEndXY.Y + (Globals.G_HUD_PADDING * 2));
            msgBoxEndXY = new Vector2Int(msgBoxStartXY.X + Globals.G_HUD_MSGBOX_W, msgBoxStartXY.Y + Globals.G_HUD_MSGBOX_H);
            msgBoxBounds = new Bounds(msgBoxStartXY, msgBoxEndXY);

            mainStatsStartXY = new Vector2Int(gwEndXY.X + (Globals.G_HUD_PADDING * 2), Globals.G_HUD_PADDING);
            mainStatsEndXY = new Vector2Int(mainStatsStartXY.X + Globals.G_HUD_MAINSTATS_W, mainStatsStartXY.Y + Globals.G_HUD_MAINSTATS_H);
            mainStatsBounds = new Bounds(mainStatsStartXY, mainStatsEndXY);

            subStatsStartXY = new Vector2Int(gwEndXY.X + (Globals.G_HUD_PADDING * 2), mainStatsEndXY.Y + (Globals.G_HUD_PADDING * 2));
            subStatsEndXY = new Vector2Int(subStatsStartXY.X + Globals.G_HUD_SUBSTATS_W, subStatsStartXY.Y + Globals.G_HUD_SUBSTATS_H);
            subStatsBounds = new Bounds(subStatsStartXY, subStatsEndXY);
        }

        // METHOD INITIALIZE WINDOW
        public void InitGameWindow()
        {
            int padding = Globals.G_HUD_PADDING;

            // SET WINDOW SIZE & BUFFER
            int WindowWidth = (gwStartXY.X - padding) + (mainStatsEndXY.X + padding);
            int WindowHeight = (gwStartXY.Y - padding) + (subStatsEndXY.Y + padding);

            Console.SetWindowSize(WindowWidth+1, WindowHeight+1);
            Console.SetBufferSize(WindowWidth+1, WindowHeight+1);

            // PRINT GAME PLAN BORDERS
            BorderPrinter(
                new Vector2Int(gwStartXY.X - padding, gwStartXY.Y - padding), 
                new Vector2Int(gwEndXY.X + padding, gwEndXY.Y + padding));

            // PRINT MESSAGE FIELD BORDERS
            BorderPrinter(
                new Vector2Int(msgBoxStartXY.X - padding, msgBoxStartXY.Y - padding), 
                new Vector2Int(msgBoxEndXY.X + padding, msgBoxEndXY.Y + padding));

            // PRINT MAIN STATS BORDERS
            BorderPrinter(
                new Vector2Int(mainStatsStartXY.X - padding, mainStatsStartXY.Y - padding), 
                new Vector2Int(mainStatsEndXY.X + padding, mainStatsEndXY.Y + padding));

            // PRINT SUB STATS BORDERS
            BorderPrinter(
                new Vector2Int(subStatsStartXY.X - padding, subStatsStartXY.Y - padding), 
                new Vector2Int(subStatsEndXY.X + padding, subStatsEndXY.Y + padding));
        }

        // METHOD BORDER PRINTER
        public void BorderPrinter(Vector2Int topleft, Vector2Int bottomright)
        {
            // PRINT BORDER HORIZONTAL AXIS
            for (int i = topleft.X; i <= bottomright.X; i++)
            {
                PrintAtPosition(i, topleft.Y, Globals.G_BORDER);
                PrintAtPosition(i, bottomright.Y, Globals.G_BORDER);
            }

            // PRINT BORDER VERTICAL AXIS
            for (int j = topleft.Y; j <= bottomright.Y; j++)
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
