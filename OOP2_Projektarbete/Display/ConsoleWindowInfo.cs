using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Display
{
    internal class ConsoleWindowInfo : IWindowInfo
    {
        public int WindowWidth => Console.WindowWidth;
        public int WindowHeight => Console.WindowHeight;
        public (int, int) CursorPosition => (Console.CursorLeft, Console.CursorTop);
    }
}
