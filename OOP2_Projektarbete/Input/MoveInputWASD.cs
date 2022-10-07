using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Input
{
    internal class MoveInputWASD : IMoveInput
    {
        public bool GetMoveInput(ConsoleKeyInfo key, out Vector2Int direction)
        {
            switch (key.Key)
            {
                case ConsoleKey.W:
                    direction = new Vector2Int(0, 1);
                    return true;
                case ConsoleKey.A:
                    direction = new Vector2Int(-1, 0);
                    return true;
                case ConsoleKey.S:
                    direction = new Vector2Int(0, -1);
                    return true;
                case ConsoleKey.D:
                    direction = new Vector2Int(1, 0);
                    return true;
                default:
                    direction = new Vector2Int(0, 0);
                    return false;
            }
        }
    }
}
