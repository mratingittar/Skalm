using OOP2_Projektarbete.Classes.Grid;
using OOP2_Projektarbete.Classes.Map;
using OOP2_Projektarbete.Classes.Structs;


namespace OOP2_Projektarbete.Classes.Managers
{
    internal class DisplayManager
    {
        private int maximumWindowWidth; // Maxium width of window for current user
        private int maximumWindowHeight; // Maximum height of window for current user
        private int windowPadding; // Cells of padding between game content and console window
        private Bounds gameBounds; // Width and height of game, i.e. consoleWindow - windowPadding
        private Bounds mapBounds;
        private Bounds messageBounds;
        //private Bounds mainStatsBounds;
        //private Bounds subStatsBounds;

        private MapManager mapManager;
        private char borderChar;

        public Bounds gameWindowBounds { get; private set; }
        public Bounds msgBoxBounds { get; private set; }
        public Bounds mainStatsBounds { get; private set; }
        public Bounds subStatsBounds { get; private set; }

        //private Grid<Cell> gameGrid;

        public DisplayManager(MapManager mapManager)
        {
            maximumWindowWidth = Console.LargestWindowWidth;
            maximumWindowHeight = Console.LargestWindowHeight;
            this.mapManager = mapManager;
            InitHudPositions();
            //gameGrid = new Grid<Cell>()
        }

        private void SetupConsoleWindow()
        {
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
        }

        private void SetWindowBorders()
        {

        }

        private void InitHudPositions()
        {
            Vector2Int gwStartXY = new Vector2Int(Globals.G_HUD_PADDING, Globals.G_HUD_PADDING);
            Vector2Int gwEndXY = new Vector2Int(gwStartXY.X + Globals.G_GAME_WIDTH, gwStartXY.Y + Globals.G_GAME_HEIGHT);
            gameWindowBounds = new Bounds(gwStartXY, gwEndXY);

            Vector2Int msgBoxStartXY = new Vector2Int(Globals.G_HUD_PADDING, gwEndXY.Y + (Globals.G_HUD_PADDING * 2));
            Vector2Int msgBoxEndXY = new Vector2Int(msgBoxStartXY.X + Globals.G_HUD_MSGBOX_W, msgBoxStartXY.Y + Globals.G_HUD_MSGBOX_H);
            msgBoxBounds = new Bounds(msgBoxStartXY, msgBoxEndXY);

            Vector2Int mainStatsStartXY = new Vector2Int(gwEndXY.X + (Globals.G_HUD_PADDING * 2), Globals.G_HUD_PADDING);
            Vector2Int mainStatsEndXY = new Vector2Int(mainStatsStartXY.X + Globals.G_HUD_MAINSTATS_W, mainStatsStartXY.Y + Globals.G_HUD_MAINSTATS_H);
            mainStatsBounds = new Bounds(mainStatsStartXY, mainStatsEndXY);

            Vector2Int subStatsStartXY = new Vector2Int(gwEndXY.X + (Globals.G_HUD_PADDING * 2), mainStatsEndXY.Y + (Globals.G_HUD_PADDING * 2));
            Vector2Int subStatsEndXY = new Vector2Int(subStatsStartXY.X + Globals.G_HUD_SUBSTATS_W, subStatsStartXY.Y + Globals.G_HUD_SUBSTATS_H);
            subStatsBounds = new Bounds(subStatsStartXY, subStatsEndXY);
        }

        public void InitGameWindow()
        {
            int padding = Globals.G_HUD_PADDING;

            // SET WINDOW SIZE & BUFFER
            int WindowWidth = (gameWindowBounds.StartXY.X - padding) + (mainStatsBounds.EndXY.X + padding);
            int WindowHeight = (gameWindowBounds.StartXY.Y - padding) + (subStatsBounds.EndXY.Y + padding);

            Console.SetWindowSize(WindowWidth + 1, WindowHeight + 1);
            Console.SetBufferSize(WindowWidth + 1, WindowHeight + 1);

            // PRINT GAME PLAN BORDERS
            BorderPrinter(
                new Vector2Int(gameWindowBounds.StartXY.X - padding, gameWindowBounds.StartXY.Y - padding),
                new Vector2Int(gameWindowBounds.EndXY.X + padding, gameWindowBounds.EndXY.Y + padding));

            // PRINT MESSAGE FIELD BORDERS
            BorderPrinter(
                new Vector2Int(msgBoxBounds.StartXY.X - padding, msgBoxBounds.StartXY.Y - padding),
                new Vector2Int(msgBoxBounds.EndXY.X + padding, msgBoxBounds.EndXY.Y + padding));

            // PRINT MAIN STATS BORDERS
            BorderPrinter(
                new Vector2Int(mainStatsBounds.StartXY.X - padding, mainStatsBounds.StartXY.Y - padding),
                new Vector2Int(mainStatsBounds.EndXY.X + padding, mainStatsBounds.EndXY.Y + padding));

            // PRINT SUB STATS BORDERS
            BorderPrinter(
                new Vector2Int(subStatsBounds.StartXY.X - padding, subStatsBounds.StartXY.Y - padding),
                new Vector2Int(subStatsBounds.EndXY.X + padding, subStatsBounds.EndXY.Y + padding));
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

        public void PrintFromPlace(int col, int row, string str)
        {
            string[] lines = str.Split("\n");

            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(col, i + row);
                Console.WriteLine(lines[i]);

            }
        }
    }
}
