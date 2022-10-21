using Skalm.GameObjects;
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
        public IPrinter Printer { get; }
        public IEraser Eraser { get; }
        public IWindowInfo WindowInfo { get; }
        public int MessagesInQueue { get => _messageQueue.Count; }
        public readonly PixelController pixelGridController;

        private SceneManager? _sceneManager;
        private Queue<string> _messageQueue;
        //private readonly Dictionary<string, char> CharSet;
        #endregion

        // CONSTRUCTOR I
        public DisplayManager(ISettings settings, IPrinter printer, IEraser eraser, IWindowInfo windowInfo, Rectangle windowSize, PixelController gridController)
        {
            Printer = printer;
            Eraser = eraser;
            WindowInfo = windowInfo;
            pixelGridController = gridController;
            _messageQueue = new Queue<string>();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //CharSet = CreateCharSet();

            windowInfo.SetWindowSize(windowSize.Width, windowSize.Height);
            Actor.OnCombatEvent += QueueMessage;
        }

        public void SetSceneManager(SceneManager sm) => _sceneManager = sm;

        public void DisplayInstantMessage(string msg) => pixelGridController.DisplayMessage(msg);

        public void DisplayNextMessage()
        {
            if (_messageQueue.Count == 0)
                return;

            pixelGridController.DisplayMessage(_messageQueue.Dequeue(), true);
        }

        public void ClearMessageSection() => pixelGridController.ClearSection("MessageSection");

        public void ClearMessageQueue() => _messageQueue.Clear();

        // DISPLAY SELECTION
        private void QueueMessage(string msg)
        {
            if (msg == "")
                return;

            _messageQueue.Enqueue(msg);
        }

        // DISPLAY HUD
        public void DisplayHUD() => pixelGridController.PrintBorders();

        public Vector2Int GetMapOrigin() => pixelGridController.GetMapOrigin();

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
