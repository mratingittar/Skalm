using Skalm.GameObjects.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class ItemEquippable : Item
    {
        public EquipSlots equipSlot;

        public StatsObject stats;

        // CONSTRUCTOR I
        public ItemEquippable(string itemName, EquipSlots equipSlot) : base(itemName)
        {
            this.equipSlot = equipSlot;
            stats = new StatsObject(0, 0, 0, 0, 0, 0, 0);
        }
    }
}
