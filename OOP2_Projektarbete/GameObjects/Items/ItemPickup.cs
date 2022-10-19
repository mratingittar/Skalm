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
        //public ItemInventory itemInventory;

        // CONSTRUCTOR I
        public ItemPickup(Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
            //this.itemInventory = itemInventory;
        }

        // INTERACT WITH ITEM
        public void Interact()
        {
            PickupItem();
        }

        // PICKUP ITEM
        private void PickupItem()
        {
            
        }
    }
}
