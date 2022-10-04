using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes.Input
{
    internal interface IMoveInput
    {
        public Vector2Int GetMoveInput();
    }
}
