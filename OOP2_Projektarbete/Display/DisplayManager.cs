using Skalm.Grid;
using Skalm.Map;
using Skalm.States;
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
        public readonly PixelController pixelGridController;
        public readonly Dictionary<string, char> CharSet;
        #endregion

        // CONSTRUCTOR I
        public DisplayManager(ISettings settings, IPrinter printer, IEraser eraser, IWindowInfo windowInfo, Rectangle windowSize, PixelController gridController)
        {
            this.printer = printer;
            this.eraser = eraser;
            this.windowInfo = windowInfo;
            pixelGridController = gridController;

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CharSet = CreateCharSet();

            windowInfo.SetWindowSize(windowSize.Width, windowSize.Height);
            PlayerStateLook.OnNeighborSelected += DisplaySelection;
        }

        // DISPLAY SELECTION
        private void DisplaySelection(string msg)
        {
            pixelGridController.DisplayMessage(msg);
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
