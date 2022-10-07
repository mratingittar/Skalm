using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Grid
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
