using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes.Input
{
    internal class InputNumpad : IMoveInput
    {
        public Vector2Int GetMoveInput()
        {
            ConsoleKeyInfo consoleKeyPressed = Console.ReadKey();

            if (consoleKeyPressed.Key ==        ConsoleKey.NumPad1) return new Vector2Int(-1, -1);
            else if (consoleKeyPressed.Key ==   ConsoleKey.NumPad2) return new Vector2Int( 0, -1);
            else if (consoleKeyPressed.Key ==   ConsoleKey.NumPad3) return new Vector2Int( 1, -1);
            else if (consoleKeyPressed.Key ==   ConsoleKey.NumPad4) return new Vector2Int(-1,  0);
            else if (consoleKeyPressed.Key ==   ConsoleKey.NumPad5) return new Vector2Int( 0,  0);  // WAIT
            else if (consoleKeyPressed.Key ==   ConsoleKey.NumPad6) return new Vector2Int( 1,  0);
            else if (consoleKeyPressed.Key ==   ConsoleKey.NumPad7) return new Vector2Int(-1,  1);
            else if (consoleKeyPressed.Key ==   ConsoleKey.NumPad8) return new Vector2Int( 0,  1);
            else if (consoleKeyPressed.Key ==   ConsoleKey.NumPad9) return new Vector2Int( 1,  1);
            else return new Vector2Int(0, 0);
        }
    }
}
