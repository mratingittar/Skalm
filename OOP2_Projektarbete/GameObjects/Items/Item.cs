using Microsoft.Win32;
using Skalm.GameObjects.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
﻿using Skalm.GameObjects.Interfaces;
using Skalm.Structs;

namespace Skalm.GameObjects.Items
{
    internal class Item
    {
        public string itemName;

        // CONSTRUCTOR I
        public Item(string itemName)
        {
            this.itemName = itemName;
        }

        // USE ITEM
        public virtual void Use(ref Player player)
        {
            
        }
    }
}
