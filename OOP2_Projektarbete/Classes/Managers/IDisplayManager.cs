using OOP2_Projektarbete.Classes.Map;
using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Managers
{
    internal interface IDisplayManager
    {
        Bounds displayBounds { get; set; }
        MapManager mapManager { get; set; }

        public void DrawField();
    }
}
