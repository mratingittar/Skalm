using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Grid
{
    internal class CellBorder : IContentType
    {
        public char Character { get; set; }
        // '█'
        public CellBorder(char character = '*')
        {
            Character = character;
        }
    }
}
