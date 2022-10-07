using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skalm.Structs;

namespace Skalm.Input
{
    internal class MoveInputNumpad : IMoveInput
    {
        public bool GetMoveInput(ConsoleKeyInfo key, out Vector2Int direction)
        {
            switch (key.Key)
            {
                case ConsoleKey.NumPad1:
                    direction = new Vector2Int(-1, -1);
                    return true;
                case ConsoleKey.NumPad2:
                    direction = new Vector2Int(0, -1);
                    return true;
                case ConsoleKey.NumPad3:
                    direction = new Vector2Int(1, -1);
                    return true;
                case ConsoleKey.NumPad4:
                    direction = new Vector2Int(-1, 0);
                    return true;
                case ConsoleKey.NumPad5:
                    direction = new Vector2Int(0, 0);
                    return true;
                case ConsoleKey.NumPad6:
                    direction = new Vector2Int(1, 0);
                    return true;
                case ConsoleKey.NumPad7:
                    direction = new Vector2Int(-1, 1);
                    return true;
                case ConsoleKey.NumPad8:
                    direction = new Vector2Int(0, 1);
                    return true;
                case ConsoleKey.NumPad9:
                    direction = new Vector2Int(1, 1);
                    return true;
                default:
                    direction = new Vector2Int(0, 0);
                    return false;
            }
        }
    }
}
