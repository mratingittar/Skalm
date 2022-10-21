using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Items
{
    internal class Key : Item
    {

        // CONSTRUCTOR I
        public Key() : base("Small key")
        {
        }

        // USE ITEM
        public override void Use(Player player)
        {
            player.equipmentManager.RemoveItemFromInventory(this);
        }

        //public Key(Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        //{
        //}

        //public void UseKey()
        //{

        //}
    }
}
