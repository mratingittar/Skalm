﻿using Skalm.GameObjects.Interfaces;
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
        public event Action<ItemPickup>? onItemPickup;

        // CONSTRUCTOR I
        public ItemPickup(Vector2Int gridPosition, char sprite, ConsoleColor color, Item item) : base(gridPosition, sprite, color)
        {
            this.item = item;
        }

        // INTERACT WITH ITEM
        public void Interact(Player player)
        {
            player.equipmentManager.AddItemToInventory(item);
            onItemPickup?.Invoke(this);
        }
    }
}