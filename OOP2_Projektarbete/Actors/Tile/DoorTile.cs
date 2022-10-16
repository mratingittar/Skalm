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
        public new char Sprite { get => ColliderIsActive ? closedSprite : openSprite; }
        private char openSprite;
        private char closedSprite;

        public DoorTile(Vector2Int gridPos, char openSprite = '□', char closedSprite = '■', ConsoleColor color = ConsoleColor.White) : base(gridPos, openSprite, color)
        {
            ObjectsOnTile = new List<IGridObject>();
            this.openSprite = openSprite;
            this.closedSprite = closedSprite;
            ColliderIsActive = true;
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
