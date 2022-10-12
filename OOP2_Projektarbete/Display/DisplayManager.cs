using Skalm.Grid;
using Skalm.Map;
using Skalm.Structs;

namespace Skalm.Display
{
    internal class DisplayManager
    {
        #region SETTINGS
        private readonly int windowPadding = Globals.G_WINDOW_PADDING;
        private readonly int borderThickness = Globals.G_BORDER_THICKNESS;
        private readonly int cellWidth = Globals.G_CELL_WIDTH;
        private readonly int cellHeight = Globals.G_CELL_HEIGHT;
        private readonly int mapWidth = Globals.G_MAP_WIDTH;
        private readonly int mapHeight = Globals.G_MAP_HEIGHT;
        private readonly int messageHeight = Math.Max(Globals.G_HUD_MSGBOX_H,3);
        private readonly int statsWidth = Globals.G_HUD_MAINSTATS_W;
        private readonly int mainStatsHeight = Globals.G_HUD_MAINSTATS_H;
        #endregion

        #region FIELDS
        // RECTANGLES
        public readonly Rectangle consoleRect;
        public readonly Rectangle gridRect;
        public readonly Rectangle gridMapRect;
        public readonly Rectangle gridMessageRect;
        public readonly Rectangle gridMainStatsRect;
        public readonly Rectangle gridSubStatsRect;

        // BOUNDS
        public readonly Bounds mapBounds;
        public readonly Bounds messageBounds;
        public readonly Bounds mainStatsBounds;
        public readonly Bounds subStatsBounds;

        // INTERFACES
        public readonly IPrinter printer;
        public readonly IEraser eraser;
        public readonly IWindowInfo windowInfo;

        public readonly GridController pixelGridController;

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

            windowInfo.SetWindowSize(consoleRect.Width, consoleRect.Height);

            pixelGridController = new GridController(new Grid2D<Pixel>(gridRect.Width, gridRect.Height, cellWidth, cellHeight, new(windowPadding * cellWidth, windowPadding * cellHeight),
                (gridX, gridY, consolePositions) => new Pixel(new(gridX, gridY), consolePositions, new HUDBorder(CharSet["shadeMedium"]))), printer, eraser);

            pixelGridController.DefineGridSections(mapBounds, new MapSection());
            pixelGridController.DefineGridSections(messageBounds, new MessageSection());
            pixelGridController.DefineGridSections(mainStatsBounds, new MainStatsSection());
            pixelGridController.DefineGridSections(subStatsBounds, new SubStatsSection());
            pixelGridController.FindBorderPixels();
            pixelGridController.DefineSectionBounds();

        }


        // DISPLAY HUD
        public void DisplayHUD()
        {
            pixelGridController.PrintBorders();
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
