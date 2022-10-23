using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class Potion : Item
    {
        public int healAmount;

        // CONSTRUCTOR I
        public Potion(string itemName, int healAmount, string itemDescription = "Heals damage") : base(itemName, $"Heals you for {healAmount} damage")
        {
            this.healAmount = healAmount;
        }

        // USE ITEM
        public override void Use(Player player)
        {
            player.statsObject.HealDamage(healAmount);
            player.equipmentManager.RemoveItemFromInventory(this);
        }

        public override int CompareTo(Item? other)
        {
            if (other == null)
                return -1;

            if (other is Potion)
                return 1;
            else if (other is ItemEquippable)
                return -1;
            else
                return 0;
        }
    }
}
