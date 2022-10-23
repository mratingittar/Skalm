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
        public Potion(string itemName, int healAmount, string itemDescription = "") : base(itemName, itemDescription)
        {
            this.healAmount = healAmount;
        }

        // USE ITEM
        public override void Use(Player player)
        {
            player.statsObject.HealDamage(healAmount);
            player.equipmentManager.RemoveItemFromInventory(this);
        }
    }
}
