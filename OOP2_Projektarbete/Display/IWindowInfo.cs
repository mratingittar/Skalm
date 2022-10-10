using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Display
{
    internal interface IWindowInfo
    {
        int WindowWidth { get; }
        int WindowHeight { get; }
        (int, int) CursorPosition { get; }
    }
}
