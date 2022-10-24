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
        public Potion(string itemName, int healAmount) : base(itemName, $"Heals you for {healAmount} damage")
        {
            this.healAmount = healAmount;
        }

        // USE ITEM
        public override void Use(Player player)
        {
            player.statsObject.HealDamage(healAmount);
            player.EquipmentManager.RemoveItemFromInventory(this);
        }

        // SORT ITEM
        public override int CompareTo(Item? other)
        {
            if (other == null)
                return -1;

            if (other is Potion)
                return 0;
            else 
                return -1;
        }
    }
}
