using Skalm.GameObjects;
using Skalm.GameObjects.Interfaces;
using Skalm.Structs;

namespace Skalm.Map.Tile
{
    internal class FloorTile : BaseTile, IOccupiable
    {
        public override char Sprite { get => ObjectsOnTile.Count == 0 ? _sprite : ObjectsOnTile.First().Sprite; }
        public override ConsoleColor Color { get => ObjectsOnTile.Count == 0 ? _color : ObjectsOnTile.First().Color; }
        public override string Label => _label;
        public Stack<GameObject> ObjectsOnTile { get; private set; }
        public bool ActorPresent { get; set; }
        private string _label = "floor";

        public FloorTile(Vector2Int gridPos, char sprite = '∙', ConsoleColor color = ConsoleColor.Gray, string label = "floor") : base(gridPos, sprite, color)
        {
            ObjectsOnTile = new Stack<GameObject>();
            ActorPresent = false;
            _label = label;
        }

    }
}
