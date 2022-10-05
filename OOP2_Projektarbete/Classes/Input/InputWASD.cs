using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Input
{
    internal class InputWASD : IMoveInput
    {
        public Vector2Int GetMoveInput()
        {
            ConsoleKeyInfo consoleKeyPressed = Console.ReadKey();

            if (consoleKeyPressed.Key == ConsoleKey.W) return new Vector2Int(0, 1);
            else if (consoleKeyPressed.Key == ConsoleKey.A) return new Vector2Int(-1, 0);
            else if (consoleKeyPressed.Key == ConsoleKey.S) return new Vector2Int(0, -1);
            else if (consoleKeyPressed.Key == ConsoleKey.D) return new Vector2Int(1, 0);
            else return new Vector2Int(0, 0);
        }
    }
}
