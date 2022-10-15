using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal class DoorTile : BaseTile, ICollider, IInteractable, IOccupiable
    {
        public bool ColliderIsActive {get; private set;}
        public List<IGridObject> ObjectsOnTile { get; private set; }

        public DoorTile(Vector2Int gridPos, char sprite = '+', ConsoleColor color = ConsoleColor.White) : base(gridPos, sprite, color)
        {
            ObjectsOnTile = new List<IGridObject>();
        }
        public override char GetSprite()
        {
            if (ObjectsOnTile.Count == 0)
                return Sprite;
            else
                return ObjectsOnTile.First().Sprite;
        }

        public override ConsoleColor GetColor()
        {
            if (ObjectsOnTile.Count == 0)
                return Color;
            else
                return ObjectsOnTile.First().Color;
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
