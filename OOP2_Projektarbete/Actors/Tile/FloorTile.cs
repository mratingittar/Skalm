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
        public List<Actor> ActorsOnTile { get; private set; }
        public FloorTile(Vector2Int gridPos, char sprite = '.', ConsoleColor color = ConsoleColor.Gray) : base(gridPos, sprite, color) 
        {
            ActorsOnTile = new List<Actor>();
            Stack<Actor> actorStack = new Stack<Actor>();
        }

        public override char GetSprite()
        {
            if (ActorsOnTile.Count == 0)
                return Sprite;
            else
                return ActorsOnTile.First().Sprite;
        }

        public override ConsoleColor GetColor()
        {
            if (ActorsOnTile.Count == 0)
                return Color;
            else
                return ActorsOnTile.First().Color;
        }
    }
}
