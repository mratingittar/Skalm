using Skalm.GameObjects;
using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Map.Tile
{
    internal class DoorTile : BaseTile, ICollider, IInteractable, IOccupiable
    {
        private char openSprite;
        private char closedSprite;

        public char DoorSprite { get => ColliderIsActive ? closedSprite : openSprite; }
        public override char Sprite { get => ObjectsOnTile.Count == 0 ? DoorSprite : ObjectsOnTile.First().Sprite; }
        public override ConsoleColor Color { get => ObjectsOnTile.Count == 0 ? _color : ObjectsOnTile.First().Color; }
        public bool ColliderIsActive { get; private set; }
        public List<IGridObject> ObjectsOnTile { get; private set; }
        public bool ActorPresent { get; set; }

        public DoorTile(Vector2Int gridPos, char openSprite = '□', char closedSprite = '■', ConsoleColor color = ConsoleColor.White) : base(gridPos, openSprite, color)
        {
            ObjectsOnTile = new List<IGridObject>();
            this.openSprite = openSprite;
            this.closedSprite = closedSprite;
            ColliderIsActive = true;
            ActorPresent = false;
        }


        public void Interact(Player player)
        {
            ColliderIsActive = !ColliderIsActive;
        }

        public void OnCollision()
        {
            throw new NotImplementedException();
        }
    }
}
