using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class ItemPickup : GameObject, IInteractable
    {
        // CONTAINED ITEM
        Item item;

        // CONSTRUCTOR I
        public ItemPickup(Vector2Int gridPosition, char sprite, ConsoleColor color, Item item) : base(gridPosition, sprite, color)
        {
            this.item = item;
        }

        // INTERACT WITH ITEM
        public void Interact(ref Player player)
        {
            PickupItem(ref player);
        }

        // PICKUP ITEM
        private void PickupItem(ref Player player)
        {
            player.equipmentManager.AddItemToInventory(item);
        }
    }
}
