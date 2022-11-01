

namespace Skalm.GameObjects.Items
{
    internal class Key : Item
    {
        // CONSTRUCTOR I
        public Key() : base("Small key", "Use to open locked doors")
        {
        }

        // USE ITEM
        public override void Use(Player player)
        {
            player.EquipmentManager.RemoveItemFromInventory(this);
        }

        // SORT ITEMS
        public override int CompareTo(Item? other)
        {
            if (other == null)
                return -1;

            if (other is Potion)
                return 1;
            else if (other is Key)
                return 0;
            else
                return -1;
        }
    }
}
