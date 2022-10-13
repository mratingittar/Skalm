using Skalm.Grid;
using Skalm.Map;
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

        public readonly PixelController pixelGridController;
        public readonly Dictionary<string, char> CharSet;

        public DisplayManager(ISettings settings, IPrinter printer, IEraser eraser, IWindowInfo windowInfo, Rectangle windowSize, PixelController gridController)
        {
            this.printer = printer;
            this.eraser = eraser;
            this.windowInfo = windowInfo;
            pixelGridController = gridController;

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CharSet = CreateCharSet();

            windowInfo.SetWindowSize(windowSize.Width, windowSize.Height);
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
    }
}
