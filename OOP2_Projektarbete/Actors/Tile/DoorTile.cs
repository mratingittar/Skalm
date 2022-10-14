using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Tile
{
    internal class DoorTile : BaseTile, ICollidable, IInteractable, IOccupiable
    {
        public bool ColliderIsActive {get; private set;}
        public List<Actor> ActorsOnTile { get; private set; }

        public DoorTile(Vector2Int gridPos, char sprite = '+', ConsoleColor color = ConsoleColor.White) : base(gridPos, sprite, color)
        {
            ActorsOnTile = new List<Actor>();
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
