using Skalm.Grid;
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
        private int messageHeight = Math.Max(Globals.G_HUD_MSGBOX_H,3);
        private int statsWidth = Globals.G_HUD_MAINSTATS_W;
        private int mainStatsHeight = Globals.G_HUD_MAINSTATS_H;
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

        private GridController gridController;
        #endregion

        public readonly Dictionary<string, char> CharSet;

        public DisplayManager(IPrinter printer, IEraser eraser, IWindowInfo windowInfo)
        {
            this.printer = printer;
            this.eraser = eraser;
            this.windowInfo = windowInfo;

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // ADD FONT CHECKING. NEEDS TO BE TRUETYPE.
            CharSet = CreateCharSet();
            DefineBounds();
            windowInfo.SetWindowSize(consoleRect.Width, consoleRect.Height);

            gridController = new GridController(new Grid2D<Pixel>(gridRect.Width, gridRect.Height, cellWidth, cellHeight, new(windowPadding * cellWidth, windowPadding * cellHeight),
                (gridX, gridY, consolePositions) => new Pixel(new(gridX, gridY), consolePositions, new HUDBorder(CharSet["shadeMedium"]))), printer, eraser);

            gridController.DefineGridSections(mapBounds, new MapSection());
            gridController.DefineGridSections(messageBounds, new MessageSection());
            gridController.DefineGridSections(mainStatsBounds, new MainStatsSection());
            gridController.DefineGridSections(subStatsBounds, new SubStatsSection());
            gridController.FindBorderPixels();
            gridController.DefineSectionBounds();
        }

        public void DisplayHUD()
        {
            gridController.PrintBorders();
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

        private Dictionary<string, char> CreateCharSet()
        {
            return new Dictionary<string, char>
            {
                {"shadeLight", '░'},
                {"shadeMedium", '▒'},
                {"shadeDark", '▓'},
                {"blockFull", '█'},
                {"boxDownRight", '╔'},
                {"boxHorizontal", '═'},
                {"boxDownLeft", '╗'},
                {"boxVertical", '║'},
                {"boxUpRight", '╚'},
                {"boxUpLeft", '╝'},
                {"pointerRight", '►'},
                {"pointerLeft", '◄'}
            };
    }

        #endregion


        public string[] AddBordersToText(string text)
        {
            string[] result = new string[3];

            result[0] = CharSet["boxDownRight"] + RepeatChar(CharSet["boxHorizontal"], text.Length) + CharSet["boxDownLeft"];
            result[1] = CharSet["boxVertical"] + text + CharSet["boxVertical"];
            result[2] = CharSet["boxUpRight"] + RepeatChar(CharSet["boxHorizontal"], text.Length) + CharSet["boxUpLeft"];

            return result;
        }

        private string RepeatChar(char ch, int count)
        {
            string result = "";
            for (int i = 0; i < count; i++)
            {
                result += ch;
            }
            return result;
        }

    }
}
