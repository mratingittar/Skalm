using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
