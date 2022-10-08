using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Menu
{
    internal class MenuPage
    {
        public readonly string pageName;
        public readonly Dictionary<int, string> items = new();

        public MenuPage(string name, params string[] items)
        {
            pageName = name;
            for (int i = 0; i < items.Length; i++)
            {
                this.items.Add(i, items[i]);
            }
        }
    }
}
