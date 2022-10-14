using Skalm.Actors.Tile;
using Skalm.Input;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors
{
    internal interface IMoveable
    {
        void Move(Vector2Int direction);
    }
}
