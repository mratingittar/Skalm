using Skalm.GameObjects.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class Item
    {
        public string itemName;
        public bool isDefault;

        // CONSTRUCTOR I
        public Item(string itemName)
        {
            this.itemName = itemName;
            isDefault = false;
        }
    }
}
