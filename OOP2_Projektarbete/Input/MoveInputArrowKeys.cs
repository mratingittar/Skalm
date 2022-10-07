using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skalm.Structs;

namespace Skalm.Input
{
    internal class MoveInputArrowKeys : IMoveInput
    {
        public bool GetMoveInput(ConsoleKeyInfo key, out Vector2Int direction)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    direction = new Vector2Int(0, 1);
                    return true;
                case ConsoleKey.LeftArrow:
                    direction = new Vector2Int(-1, 0);
                    return true;
                case ConsoleKey.DownArrow:
                    direction = new Vector2Int(0, -1);
                    return true;
                case ConsoleKey.RightArrow:
                    direction = new Vector2Int(1, 0);
                    return true;
                default:
                    direction = new Vector2Int(0, 0);
                    return false;
            }
        }
    }
}
