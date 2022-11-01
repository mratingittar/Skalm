using Skalm.GameObjects.Interfaces;
using Skalm.Structs;


namespace Skalm.GameObjects.Items
{
    internal class ItemPickup : GameObject, IInteractable
    {
        // CONTAINED ITEM
        Item item;
        public override string Label { get => item.Name; }

        // STATIC EVENT - ON ANY ITEM PICKED UP
        public static event Action<ItemPickup>? onItemPickup;

        // CONSTRUCTOR I
        public ItemPickup(Vector2Int gridPosition, char sprite, ConsoleColor color, Item item) : base(gridPosition, sprite, color)
        {
            this.item = item;
        }

        // INTERACT WITH ITEM
        public void Interact(Player player)
        {
            player.EquipmentManager.AddItemToInventory(item);
            onItemPickup?.Invoke(this);
        }
    }
}
