using Skalm.GameObjects;
using Skalm.GameObjects.Interfaces;
using Skalm.Structs;

namespace Skalm.Map.Tile
{
    internal class DoorTile : BaseTile, ICollider, IInteractable, IOccupiable
    {
        private char _openSprite;
        private char _closedSprite;
        private string _label;

        public char DoorSprite { get => ColliderIsActive ? _closedSprite : _openSprite; }
        public override char Sprite { get => ObjectsOnTile.Count == 0 ? DoorSprite : ObjectsOnTile.First().Sprite; }
        public override ConsoleColor Color { get => ObjectsOnTile.Count == 0 ? _color : ObjectsOnTile.First().Color; }
        public bool ColliderIsActive { get; private set; }
        public bool IsLocked { get; private set; }
        public Stack<GameObject> ObjectsOnTile { get; private set; }
        public bool ActorPresent { get; set; }
        public override string Label
        {
            get
            {
                string l;
                if (ColliderIsActive)
                    l = "closed ";
                else
                    l = "Open ";

                if (IsLocked)
                    l += "and locked ";
                l += "door.";

                if (IsLocked)
                    l += " Unlock it?";
                else if (ColliderIsActive)
                    l += " Open it?";
                return l;
            }
        }

        public DoorTile(Vector2Int gridPos, char openSprite = '□', char closedSprite = '■', ConsoleColor color = ConsoleColor.White) : base(gridPos, openSprite, color)
        {
            ObjectsOnTile = new Stack<GameObject>();
            this._openSprite = openSprite;
            this._closedSprite = closedSprite;
            ColliderIsActive = true;
            IsLocked = true;
            ActorPresent = false;
            _label = string.Empty;
        }


        public void Interact(Player player)
        {
            if (IsLocked) // WILL REQUIRE KEY
            { 
                IsLocked = false;
                ColliderIsActive = false;
            }
            else
                ColliderIsActive = !ColliderIsActive;
        }

        public void OnCollision()
        {
            
        }
    }
}
