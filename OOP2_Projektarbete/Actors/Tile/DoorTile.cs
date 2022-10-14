using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal class DoorTile : BaseTile, ICollidable, IInteractable
    {
        public bool ColliderIsActive {get; private set;}
        public DoorTile(Vector2Int gridPos, char sprite = '+', ConsoleColor color = ConsoleColor.White) : base(gridPos, sprite, color)
        {
        }


        public void Interact()
        {
            ColliderIsActive = !ColliderIsActive;
        }

        public void OnCollision()
        {
            throw new NotImplementedException();
        }
    }
}
