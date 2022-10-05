using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Structs
{
    internal struct Bounds2Int
    {
        public Vector2Int startXY;
        public Vector2Int endXY;

        public Bounds2Int(Vector2Int startXY, Vector2Int endXY)
        {
            this.startXY = startXY;
            this.endXY = endXY;
        }
    }
}
