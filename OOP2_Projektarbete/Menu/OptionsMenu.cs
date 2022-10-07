using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Menu
{
    internal class OptionsMenu : IMenu
    {
        public string MenuName { get; private set; }
        public bool Enabled { get; set; }

        public MenuItems Items { get; private set; }

        public OptionsMenu(string name, MenuItems items)
        {
            MenuName = name;
            Items = items;
        }
    }
}
