using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Menu
{
    internal interface IMenu
    {
        string MenuName { get; }
        bool Enabled { get; set; }
        MenuItems Items { get; }
    }
}
