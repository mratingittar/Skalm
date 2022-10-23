using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class Key : Item
    {
        // CONSTRUCTOR I
        public Key() : base("Small key", "Use to open locked doors")
        {
        }

        // USE ITEM
        public override void Use(Player player)
        {
            player.equipmentManager.RemoveItemFromInventory(this);
        }

        public override int CompareTo(Item? other)
        {
            if (other == null)
                return -1;
            else
            return -1;
        }
    }
}
