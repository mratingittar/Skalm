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
        // CONTAINERS
        ItemEquippable[] equipArr;
        Inventory inventory;

        // EVENT
        public event Action? onEquippmentChanged;

        // CONSTRUCTOR I
        public EquipmentManager()
        {
            equipArr = new ItemEquippable[Enum.GetValues(typeof(EEqSlots)).Length];
            inventory = new Inventory();
        }

        // ADD & REMOVE ITEM TO/FROM INVENTORY
        public void AddItemToInventory(Item item) => inventory.AddItem(item);
        public void RemoveItemFromInventory(Item item) => inventory.RemoveItem(item);

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
                    //oldItem.stats.statsArr.ForEach(x => playerStats.statsArr[(int)x.statName].RemoveModifier(x.GetValue()));
                    foreach (var itemStat in oldItem.stats.statsArr)
                        playerStats.statsArr[(int)itemStat.statName].RemoveModifier(itemStat.GetValue());

                    inventory.AddItem(oldItem);
                }

                // ADD NEW ITEM STATS TO PLAYER STATS OBJECT
                foreach (var itemStat in item.stats.statsArr)
                    playerStats.statsArr[(int)itemStat.statName].AddModifier(itemStat.GetValue());

                // FIRE EVENTS
                onEquippmentChanged?.Invoke();
            }
        }
    }

    // ENUM EQUIPMENT SLOTS
    public enum EEqSlots
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
