using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Menu
{
    internal struct MenuItems
    {
        public readonly Dictionary<int, string> Values;

        public MenuItems(params string[] menuItems)
        {
            Values = new Dictionary<int, string>();
            for (int i = 0; i < menuItems.Length; i++)
            {
                Values[i] = menuItems[i];
            }
        }
    }
}
