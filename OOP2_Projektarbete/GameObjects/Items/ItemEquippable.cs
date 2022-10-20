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
        public int equipSlot;
        public bool isDefault;

        // ITEM STATS
        public StatsObject stats;

        // CONSTRUCTOR I
        public ItemEquippable(string itemName, int equipSlot) : base(itemName)
        {
            this.equipSlot = equipSlot;
            stats = new StatsObject(0, 0, 0, 0, 0, 0, 0, 0);
        }

        // CONSTRUCTOR II
        public ItemEquippable(string itemName, int equipSlot, StatsObject stats) : base(itemName)
        {
            this.equipSlot = equipSlot;
            this.stats = stats;
        }

        // USE ITEM
        public override void Use(ref Player player)
        {
            // EQUIP ITEM
            player.equipmentManager.EquipItem(ref player.statsObject.stats, this);
        }
    }
}
