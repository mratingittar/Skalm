using Skalm.GameObjects;
using Skalm.Grid;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Display
{
    internal class DisplayManager
    {
        public IPrinter Printer { get; }
        public IEraser Eraser { get; }
        public IWindowInfo WindowInfo { get; }
        public int MessagesInQueue { get => _messageQueue.Count; }
        public PixelController PixelGridController => _pixelGridController;

        private readonly PixelController _pixelGridController;
        private Queue<string> _messageQueue;

        // CONSTRUCTOR I
        public DisplayManager(ISettings settings, IPrinter printer, IEraser eraser, IWindowInfo windowInfo, Rectangle windowSize, PixelController gridController)
        {
            Printer = printer;
            Eraser = eraser;
            WindowInfo = windowInfo;
            _pixelGridController = gridController;
            _messageQueue = new Queue<string>();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            windowInfo.SetWindowSize(windowSize.Width, windowSize.Height);
            Actor.OnCombatEvent += QueueMessage;
        }

        public void DisplayInstantMessage(string msg) => _pixelGridController.DisplayMessage(msg);

        public void DisplayNextMessage()
        {
            if (_messageQueue.Count == 0)
                return;

            _pixelGridController.DisplayMessage(_messageQueue.Dequeue(), true);
        }

        public void ClearMessageSection() => _pixelGridController.ClearSection("MessageSection");

        public void ClearMessageQueue() => _messageQueue.Clear();

        // DISPLAY SELECTION
        private void QueueMessage(string msg)
        {
            if (msg == "")
                return;

            _messageQueue.Enqueue(msg);
        }

        // DISPLAY HUD
        public void DisplayHUD() => _pixelGridController.PrintBorders();

        public Vector2Int GetMapOrigin() => _pixelGridController.GetMapOrigin();
    }
}
