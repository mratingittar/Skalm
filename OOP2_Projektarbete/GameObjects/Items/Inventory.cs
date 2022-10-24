using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class Inventory
    {
        public event Action? OnInventoryChanged;

        // INVENTORY ITEM LIST
        public List<Item> itemList;
        public int Keys;

        // CONSTRUCTOR I
        public Inventory()
        {
            this.itemList = new List<Item>();
            Keys = 0;
        }

        // ADD ITEM TO INVENTORY
        public void AddItem(Item item)
        {
            if (item != null)
            {
                itemList.Add(item);
                SortInventory();
                OnInventoryChanged?.Invoke();

                if (item is Key)
                    Keys++;
            }
        }

        // REMOVE ITEM FROM INVENTORY
        public void RemoveItem(Item item)
        {
            if (item != null)
            {
                itemList.Remove(item);
                OnInventoryChanged?.Invoke();

                if (item is Key)
                    Keys--;
            }
        }

        // SORT INVENTORY
        private void SortInventory()
        {
            itemList.Sort();
        }
    }
}
