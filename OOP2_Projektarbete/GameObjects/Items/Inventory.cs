using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class Inventory
    {
        public event Action? onInventoryChanged;

        // INVENTORY ITEM LIST
        public List<Item> itemList;

        // CONSTRUCTOR I
        public Inventory()
        {
            this.itemList = new List<Item>();
        }

        // ADD ITEM TO INVENTORY
        public void AddItem(Item item)
        {
            if (item != null)
            {
                itemList.Add(item);
                onInventoryChanged?.Invoke();
            }
        }

        // REMOVE ITEM FROM INVENTORY
        public void RemoveItem(Item item)
        {
            if (item != null)
            {
                itemList.Remove(item);
                onInventoryChanged?.Invoke();
            }
        }
    }
}
