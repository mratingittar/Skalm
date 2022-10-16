using Skalm.Map;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal class FloorTile : BaseTile, IOccupiable
    {
        public List<IGridObject> ObjectsOnTile { get; private set; }
        public FloorTile(Vector2Int gridPos, char sprite = '∙', ConsoleColor color = ConsoleColor.Gray) : base(gridPos, sprite, color) 
        {
            ObjectsOnTile = new List<IGridObject>();
            //Stack<Actor> actorStack = new Stack<Actor>();
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
    }
}
