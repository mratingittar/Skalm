using Skalm.GameObjects;
using Skalm.GameObjects.Interfaces;
using Skalm.Structs;

namespace Skalm.Map.Tile
{
    internal class FloorTile : BaseTile, IOccupiable
    {
        public override char Sprite { get => ObjectsOnTile.Count == 0 ? _sprite : ObjectsOnTile.First().Sprite; }
        public override ConsoleColor Color { get => ObjectsOnTile.Count == 0 ? _color : ObjectsOnTile.First().Color; }
        public List<GameObject> ObjectsOnTile { get; private set; }
        public bool ActorPresent { get; set; }

        public FloorTile(Vector2Int gridPos, char sprite = '∙', ConsoleColor color = ConsoleColor.Gray) : base(gridPos, sprite, color)
        {
            ObjectsOnTile = new List<GameObject>();
            ActorPresent = false;
            //Stack<Actor> actorStack = new Stack<Actor>();
        }

    }
}
