using Skalm.Grid;
using Skalm.Structs;

namespace Skalm.Display
{
    internal class DisplayManager
    {
        #region FIELDS
        private readonly int verticalBorders = 3;
        private readonly int horizontalBorders = 3;

        // RECTANGLES
        private readonly Rectangle consoleRect;
        private readonly Rectangle gridRect;
        private readonly Rectangle gridMapRect;
        private readonly Rectangle gridMessageRect;
        private readonly Rectangle gridMainStatsRect;
        private readonly Rectangle gridSubStatsRect;

        // BOUNDS
        private readonly Bounds mapBounds;
        private readonly Bounds messageBounds;
        private readonly Bounds mainStatsBounds;
        private readonly Bounds subStatsBounds;

        // INTERFACES
        public readonly IPrinter printer;
        public readonly IEraser eraser;
        public readonly IWindowInfo windowInfo;

        public readonly GridController pixelGridController;
        #endregion

        public readonly Dictionary<string, char> CharSet;

        public DisplayManager(ISettings settings, IPrinter printer, IEraser eraser, IWindowInfo windowInfo)
        {
            this.printer = printer;
            this.eraser = eraser;
            this.windowInfo = windowInfo;

            

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // ADD FONT CHECKING. NEEDS TO BE TRUETYPE.
            CharSet = CreateCharSet();

            gridMapRect = new Rectangle(settings.MapWidth, settings.MapHeight);
            gridMessageRect = new Rectangle(settings.MapWidth, settings.MessageBoxHeight);
            gridMainStatsRect = new Rectangle(settings.StatsWidth, settings.MainStatsHeight);
            gridSubStatsRect = new Rectangle(settings.StatsWidth, settings.MapHeight + settings.MessageBoxHeight - settings.MainStatsHeight);
            gridRect = new Rectangle(gridMapRect.Width + gridMainStatsRect.Width + settings.BorderThickness * verticalBorders,
                gridMapRect.Height + gridMessageRect.Height + settings.BorderThickness * horizontalBorders);
            consoleRect = new Rectangle((gridRect.Width + settings.WindowPadding * 2) * settings.CellWidth, (gridRect.Height + settings.WindowPadding * 2) * settings.CellHeight);

            mapBounds = new Bounds(new Vector2Int(settings.BorderThickness, settings.BorderThickness), gridMapRect);
            messageBounds = new Bounds(new Vector2Int(settings.BorderThickness, mapBounds.EndXY.Y + settings.BorderThickness), gridMessageRect);
            mainStatsBounds = new Bounds(new Vector2Int(mapBounds.EndXY.X + settings.BorderThickness, mapBounds.StartXY.Y), gridMainStatsRect);
            subStatsBounds = new Bounds(new Vector2Int(mainStatsBounds.StartXY.X, mainStatsBounds.EndXY.Y + settings.BorderThickness), gridSubStatsRect);

            windowInfo.SetWindowSize(consoleRect.Width, consoleRect.Height);

            pixelGridController = new GridController(new Grid2D<Pixel>(gridRect.Width, gridRect.Height, settings.CellWidth, settings.CellHeight, 
                new Vector2Int(settings.WindowPadding * settings.CellWidth, settings.WindowPadding * settings.CellHeight),
                (gridX, gridY, consolePositions) => new Pixel(new(gridX, gridY), consolePositions, new HUDBorder(CharSet["shadeMedium"]))), printer, eraser);

            pixelGridController.DefineGridSections(mapBounds, new MapSection());
            pixelGridController.DefineGridSections(messageBounds, new MessageSection());
            pixelGridController.DefineGridSections(mainStatsBounds, new MainStatsSection());
            pixelGridController.DefineGridSections(subStatsBounds, new SubStatsSection());
            pixelGridController.FindBorderPixels();
            pixelGridController.DefineSectionBounds();

        }

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
