using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Grid
{
    internal class CellEmpty : IContentType
    {
        public char Character { get; set; }

        public CellEmpty()
        {
            Character = ' ';
        }
    }
}
