using Skalm.Map;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Display
{
    internal interface IDisplayManager
    {
        Bounds displayBounds { get; set; }
        MapManager mapManager { get; set; }

        public void DrawField();
    }
}
