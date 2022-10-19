using Skalm.GameObjects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal class EquipmentManager
    {
        ItemEquippable[] equipArr;
        Inventory inventory;

        // CONSTRUCTOR I
        public EquipmentManager()
        {
            equipArr = new ItemEquippable[Enum.GetValues(typeof(EquipSlots)).Length];
            inventory = new Inventory();
        }

        // EQUIP ITEM
        public void EquipItem(ref StatsObject playerStats, ItemEquippable item)
        {
            if (item != null)
            {
                // CACHE OLD ITEM & EQUIP NEW ITEM
                ItemEquippable oldItem = equipArr[item.equipSlot];
                equipArr[item.equipSlot] = item;

                // IF OLD ITEM NOT NULL, REMOVE STAT BONUSES & ADD TO INVENTORY
                if (oldItem != null)
                {
                    foreach (var itemStat in oldItem.stats.statsArr)
                        playerStats.statsArr[(int)itemStat.statName].RemoveModifier(itemStat.GetValue());

                    inventory.AddItem(oldItem);
                }

                // ADD NEW ITEM STATS TO PLAYER STATS OBJECT
                foreach (var itemStat in item.stats.statsArr)
                    playerStats.statsArr[(int)itemStat.statName].AddModifier(itemStat.GetValue());
            }
        }
    }

    // ENUM EQUIPMENT SLOTS
    public enum EquipSlots
    {
        Head,
        Torso,
        LHand,
        RHand,
        LFinger,
        RFinger,
        Feet
    }
}
