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

        // INTERFACES
        public readonly IPrinter printer;
        public readonly IEraser eraser;
        public readonly IWindowInfo windowInfo;

        private GridController gameGridController;
        #endregion

        public DisplayManager(IPrinter printer, IEraser eraser, IWindowInfo windowInfo)
        {
            this.printer = printer;
            this.eraser = eraser;
            this.windowInfo = windowInfo;

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // ADD FONT CHECKING. NEEDS TO BE TRUETYPE.

            DefineBounds();
            windowInfo.SetWindowSize(consoleRect.Width, consoleRect.Height);

            gameGridController = new GridController(new Grid2D<Cell>(gridRect.Width, gridRect.Height, cellWidth, cellHeight, new(windowPadding * cellWidth, windowPadding * cellHeight),
                (gridX, gridY, consolePositions) => new Cell(new(gridX, gridY), consolePositions, new HUDBorder('░'))), printer, eraser);

            gameGridController.DefineGridSections(mapBounds, new MapSection());
            gameGridController.DefineGridSections(messageBounds, new MessageSection());
            gameGridController.DefineGridSections(mainStatsBounds, new MainStatsSection());
            gameGridController.DefineGridSections(subStatsBounds, new SubStatsSection());

            gameGridController.FindBorderCells();

        }

        public void DisplayHUD()
        {
            gameGridController.PrintBorders();
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
        }
        #endregion

    }
}
