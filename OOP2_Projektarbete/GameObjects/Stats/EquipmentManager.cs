using Skalm.GameObjects.Items;


namespace Skalm.GameObjects.Stats
{
    internal class EquipmentManager
    {
        // CONTAINERS
        public ItemEquippable[] equipArr;
        public Inventory inventory;

        // CONSTRUCTOR I
        public EquipmentManager()
        {
            equipArr = new ItemEquippable[Enum.GetValues(typeof(EEqSlots)).Length];
            inventory = new Inventory();

            for (int i = 0; i < equipArr.Length; i++)
            {
                equipArr[i] = new ItemEquippable("-----", i, true);
            }
        }

        // RESET INVENTORY
        public void ResetInventory()
        {
            inventory.itemList.Clear();
            for (int i = 0; i < equipArr.Length; i++)
            {
                equipArr[i] = new ItemEquippable("-----", i, true);
            }
        }

        // ADD & REMOVE ITEM TO/FROM INVENTORY
        public void AddItemToInventory(Item item) => inventory.AddItem(item);
        public void RemoveItemFromInventory(Item item) => inventory.RemoveItem(item);

        // EQUIP ITEM
        public void EquipItem(StatsObject playerStats, ItemEquippable item)
        {
            if (item != null)
            {
                // CACHE OLD ITEM & EQUIP NEW ITEM
                ItemEquippable oldItem = equipArr[item.equipSlot];
                equipArr[item.equipSlot] = item;

                // IF OLD ITEM NOT NULL OR DEFAULT, REMOVE STAT BONUSES & ADD TO INVENTORY
                if (oldItem != null && oldItem.isDefault is false)
                {
                    foreach (var itemStat in oldItem.stats.statsArr)
                        playerStats.statsArr[(int)itemStat.statName].RemoveModifier(itemStat.GetValue());
                        inventory.AddItem(oldItem);
                }

                // ADD NEW ITEM STATS TO PLAYER STATS OBJECT
                foreach (var itemStat in item.stats.statsArr)
                    playerStats.statsArr[(int)itemStat.statName].AddModifier(itemStat.GetValue());
                inventory.RemoveItem(item);
            }
        }
    }

    // ENUM EQUIPMENT SLOTS
    public enum EEqSlots
    {
        Head,
        Torso,
        RHand,
        LHand,
        Legs,
        Feet,
        Finger,
    }
}
