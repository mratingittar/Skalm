using Skalm.Grid;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Display
{
    internal class DisplayManager
    {
        #region FIELDS
        public readonly IPrinter printer;
        public readonly IEraser eraser;
        public readonly IWindowInfo windowInfo;
        #endregion

        public readonly Dictionary<string, Bounds> sectionBounds;
        public readonly GridController pixelGridController;
        public readonly Dictionary<string, char> CharSet;

        public DisplayManager(ISettings settings, IPrinter printer, IEraser eraser, IWindowInfo windowInfo, Rectangle windowSize, Dictionary<string, Bounds> sectionBounds, GridController gridController)
        {
            this.printer = printer;
            this.eraser = eraser;
            this.windowInfo = windowInfo;
            this.sectionBounds = sectionBounds;
            pixelGridController = gridController;

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CharSet = CreateCharSet();

            windowInfo.SetWindowSize(windowSize.Width, windowSize.Height);

            pixelGridController.DefineGridSections(sectionBounds["mapBounds"], new MapSection());
            pixelGridController.DefineGridSections(sectionBounds["messageBounds"], new MessageSection());
            pixelGridController.DefineGridSections(sectionBounds["mainStatsBounds"], new MainStatsSection());
            pixelGridController.DefineGridSections(sectionBounds["subStatsBounds"], new SubStatsSection());
            pixelGridController.FindBorderPixels();
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
