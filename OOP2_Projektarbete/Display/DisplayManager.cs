using Skalm.Grid;
using Skalm.Map;
using Skalm.Structs;

namespace Skalm.Display
{
    internal class DisplayManager
    {
        #region SETTINGS
        private int windowPadding = Globals.G_WINDOW_PADDING;
        private int borderThickness = Globals.G_BORDER_THICKNESS;
        private int cellWidth = Globals.G_CELL_WIDTH;
        private int cellHeight = Globals.G_CELL_HEIGHT;
        private int mapWidth = Globals.G_MAP_WIDTH;
        private int mapHeight = Globals.G_MAP_HEIGHT;
        private int messageHeight = Globals.G_HUD_MSGBOX_H;
        private int statsWidth = Globals.G_HUD_MAINSTATS_W;
        private int mainStatsHeight = Globals.G_HUD_MAINSTATS_H;
        private char borderChar = Globals.G_BORDER_CHAR;

        private static ConsoleColor backgroundColor = ConsoleColor.Black;
        private static ConsoleColor foregroundColor = ConsoleColor.White;
        #endregion

        #region FIELDS
        // RECTANGLES
        private Rectangle consoleRect;
        private Rectangle gridRect;
        private Rectangle gridMapRect;
        private Rectangle gridMessageRect;
        private Rectangle gridMainStatsRect;
        private Rectangle gridSubStatsRect;

        // BOUNDS
        private Bounds mapBounds;
        private Bounds messageBounds;
        private Bounds mainStatsBounds;
        private Bounds subStatsBounds;
        private static Bounds latestPrintedArea;

        // MANAGERS
        private MapManager mapManager;

        //public static (Bounds, string[]) cachedCharacters; NOT IMPLEMENTED

        #endregion

        #region PROPERTIES
        public Grid<Cell> GameGrid { get; private set; }
        public Dictionary<IContentType, Cell> CellContents { get; private set; } // USED TO SAVE POSITIONS OF DIFFERENT TYPES OF CELLS FOR LATER ACCESS
        #endregion

        public DisplayManager(MapManager mapManager)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // ADD FONT CHECKING. NEEDS TO BE TRUETYPE.
            DefineBounds();
            CellContents = new Dictionary<IContentType, Cell>();
            GameGrid = new Grid<Cell>(gridRect.Width, gridRect.Height, cellWidth, cellHeight, new(windowPadding * cellWidth, windowPadding * cellHeight),
                (gridPosition, consolePositions) => new Cell(gridPosition, consolePositions, new CellBorder(borderChar), false));

            GameGrid.DefineContentOfGridArea(mapBounds, new CellEmpty());
            GameGrid.DefineContentOfGridArea(messageBounds, new CellEmpty());
            GameGrid.DefineContentOfGridArea(mainStatsBounds, new CellEmpty());
            GameGrid.DefineContentOfGridArea(subStatsBounds, new CellEmpty());
            this.mapManager = mapManager;
            latestPrintedArea = messageBounds;
        }

        public void DisplayHUD()
        {
            GameGrid.PrintGridContent(new CellBorder());
        }

        #region SETUP
        private void DefineBounds()
        {
            gridMapRect = new Rectangle(mapWidth, mapHeight);
            gridMessageRect = new Rectangle(mapWidth, messageHeight);
            gridMainStatsRect = new Rectangle(statsWidth, mainStatsHeight);
            gridSubStatsRect = new Rectangle(statsWidth, mapHeight + messageHeight - mainStatsHeight);
            gridRect = new Rectangle(gridMapRect.Width + gridMainStatsRect.Width + borderThickness * 3,
                gridMapRect.Height + gridMessageRect.Height + borderThickness * 3);
            consoleRect = new Rectangle((gridRect.Width + windowPadding * 2) * cellWidth, (gridRect.Height + windowPadding * 2) * cellHeight);

            mapBounds = new Bounds(new Vector2Int(borderThickness, borderThickness), gridMapRect);
            messageBounds = new Bounds(new Vector2Int(borderThickness, mapBounds.EndXY.Y + borderThickness), gridMessageRect);
            mainStatsBounds = new Bounds(new Vector2Int(mapBounds.EndXY.X + borderThickness, mapBounds.StartXY.Y), gridMainStatsRect);
            subStatsBounds = new Bounds(new Vector2Int(mainStatsBounds.StartXY.X, mainStatsBounds.EndXY.Y + borderThickness), gridSubStatsRect);


            try
            {
                if (consoleRect.Width > Console.LargestWindowWidth || consoleRect.Height > Console.LargestWindowHeight)
                    throw new ArgumentOutOfRangeException("Game window does not fit inside monitor.");
                
                Console.SetWindowSize(consoleRect.Width, consoleRect.Height);
                Console.SetBufferSize(consoleRect.Width, consoleRect.Height);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
            }
        }
        #endregion

        #region PRINTING
        // METHOD BORDER PRINTER
        public void BorderPrinter(Vector2Int topleft, Vector2Int bottomright)
        {
            // PRINT BORDER HORIZONTAL AXIS
            for (int i = topleft.X; i <= bottomright.X; i++)
            {
                PrintAtPosition(i, topleft.Y, Globals.G_BORDER_CHAR);
                PrintAtPosition(i, bottomright.Y, Globals.G_BORDER_CHAR);
            }

            // PRINT BORDER VERTICAL AXIS
            for (int j = topleft.Y; j <= bottomright.Y; j++)
            {
                PrintAtPosition(topleft.X, j, Globals.G_BORDER_CHAR);
                PrintAtPosition(bottomright.X, j, Globals.G_BORDER_CHAR);
            }
        }

        // METHOD PRINT AT POSITION
        public static void PrintAtPosition(int x, int y, char character)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(character);
        }

        /// <summary>
        /// Prints string at row, starting at columnStart. Option to highlight string.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="row"></param>
        /// <param name="columnStart"></param>
        /// <param name="highlighted">If true, prints string with inverted colors.</param>
        public static void Print(string line, int row, int columnStart, bool highlighted = false)
        {
            if (highlighted)
            {
                Console.BackgroundColor = foregroundColor;
                Console.ForegroundColor = backgroundColor;
            }

            Console.SetCursorPosition(columnStart, row);
            Console.Write(line);

            if (highlighted)
            {
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = foregroundColor;
            }
        }

        /// <summary>
        /// Prints string array, starting at rowStart and colStart. Option to highlight strings.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="rowStart"></param>
        /// <param name="columnStart"></param>
        /// <param name="highlighted">If true, prints string with inverted colors.</param>
        public static void Print(string[] lines, int rowStart, int columnStart, bool highlighted = false)
        {
            latestPrintedArea = new Bounds(new Vector2Int(columnStart, rowStart), new Rectangle(lines.Max(line => line.Length), lines.Length));
            for (int i = 0; i < lines.Length; i++)
                Print(lines[i], rowStart + i, columnStart, highlighted);
        }

        /// <summary>
        /// Prints string at center of console and specified row. Option to highlight string.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="row"></param>
        /// <param name="highlighted">If true, prints string with inverted colors.</param>
        public static void PrintCentered(string line, int row, bool highlighted = false)
        {
            Print(line, row, FindOffsetFromConsoleCenter(line.Length), highlighted);
        }

        /// <summary>
        /// Prints string array at center of console, starting from rowStart. Option to highlight strings.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="rowStart"></param>
        /// <param name="highlighted">If true, prints string with inverted colors.</param>
        public static void PrintCentered(string[] lines, int rowStart, bool highlighted = false)
        {
            int width = lines.Max(line => line.Length);
            Vector2Int startPos = new Vector2Int(FindOffsetFromConsoleCenter(width), rowStart);
            latestPrintedArea = new Bounds(startPos, new Rectangle(width, lines.Length));

            for (int i = 0; i < lines.Length; i++)
            {
                PrintCentered(lines[i], rowStart + i, highlighted);
            }
        }

        private static int FindOffsetFromConsoleCenter(int width)
        {
            return (int)((float)Console.WindowWidth / 2 - (float)width / 2);
        }


        // NOT WORKING, MISSING CONSOLE READING
        private (Bounds, string[]) CacheCharactersInArea(Bounds area)
        {
            List<string> allLines = new();
            for (int rows = area.StartXY.Y; rows < area.EndXY.Y; rows++)
            {
                string line = "";
                for (int cols = area.StartXY.X; cols < area.EndXY.X; cols++)
                {
                    // READING FROM CONSOLE IS COMPLICATED
                    // MAYBE LATER
                }
                allLines.Add(line);
            }
            return (area, allLines.ToArray());
        }

        public static void ReplaceAreaWithStringArray(Bounds area, string[] lines)
        {
            Erase(area);
            for (int i = 0; i < lines.Length; i++)
            {
                Print(lines[i], 0, area.StartXY.Y + i, true);
            }
        }

        public static void EraseLatestPrint()
        {
            Erase(latestPrintedArea);
        }

        public static void Erase(Bounds area)
        {
            for (int x = area.StartXY.X; x < area.EndXY.X; x++)
            {
                for (int y = area.StartXY.Y; y < area.EndXY.Y; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }
        }

        #endregion
    }
}
