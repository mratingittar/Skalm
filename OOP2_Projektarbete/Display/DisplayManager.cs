﻿using Skalm.Grid;
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

        // INTERFACES
        public readonly IPrinter printer;
        public readonly IEraser eraser;
        public readonly IWindowInfo windowInfo;
        #endregion

        #region PROPERTIES
        public Grid<Cell> GameGrid { get; private set; }
        public Dictionary<IContentType, Cell> CellContents { get; private set; } // USED TO SAVE POSITIONS OF DIFFERENT TYPES OF CELLS FOR LATER ACCESS
        #endregion

        public DisplayManager(IPrinter printer, IEraser eraser, IWindowInfo windowInfo)
        {
            this.printer = printer;
            this.eraser = eraser;
            this.windowInfo = windowInfo;

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

        // METHOD PRINT AT POSITION
        public static void PrintAtPosition(int x, int y, char character)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(character);
        }


        public void EraseLatestPrint()
        {
            eraser.EraseArea(latestPrintedArea);
        }

        #endregion
    }
}
