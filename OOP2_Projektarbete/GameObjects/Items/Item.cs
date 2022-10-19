using Skalm.GameObjects.Interfaces;
using Skalm.Structs;


namespace Skalm.GameObjects.Items
{
    internal class Item : GameObject, IInteractable
    {
        public Item(Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
        }

        public void Interact()
        {
            PickupItem();
        }

        private void PickupItem()
        {
            
        }
    }
}
