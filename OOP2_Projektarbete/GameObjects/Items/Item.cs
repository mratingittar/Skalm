using Microsoft.Win32;
using Skalm.GameObjects.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal abstract class Item
    {
        public string itemName;

        // CONSTRUCTOR I
        public Item(string itemName)
        {
            this.itemName = itemName;
        }

        // USE ITEM
        //public abstract void Use();
    }
}
