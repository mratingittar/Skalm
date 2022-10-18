using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Interfaces
{
    internal interface IMoveBehaviour
    {
        Vector2Int MoveDirection(Vector2Int currentPosition);
    }
}
