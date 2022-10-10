using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Display
{
    internal interface IPrinter
    {
        public void PrintFromPosition(string line, int y, int x, bool highlighted = false);
        public void PrintFromPosition(string[] lines, int y, int x, bool highlighted = false);
        public void PrintCenteredInWindow(string line, int y, bool highlighted = false);
        public void PrintCenteredInWindow(string[] line, int y, bool highlighted = false);        
    }
}
