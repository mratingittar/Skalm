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
        IMoveInput _moveInput { get; set; }

        void Move(Vector2Int target);
    }
}
