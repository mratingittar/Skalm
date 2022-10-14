using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Animation
{
    internal class Frame
    {
        public string[] Lines { get; private set; }
        public int Width { get; private set; }

        public Frame(params string[] lines)
        {
            Lines = lines;
            Width = Lines.Select(x => x.Length).Max();
        }
    }
}
