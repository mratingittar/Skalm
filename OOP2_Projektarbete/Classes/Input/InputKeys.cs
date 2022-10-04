using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes.Input
{
    internal class InputKeys : IMoveInput
    {
        public Vector2Int GetMoveInput()
        {
            ConsoleKeyInfo consoleKeyPressed = Console.ReadKey();

            if (consoleKeyPressed.Key ==        ConsoleKey.RightArrow)  return new Vector2Int( 1,  0);
            else if (consoleKeyPressed.Key ==   ConsoleKey.LeftArrow)   return new Vector2Int(-1,  0);
            else if (consoleKeyPressed.Key ==   ConsoleKey.UpArrow)     return new Vector2Int( 0,  1);
            else if (consoleKeyPressed.Key ==   ConsoleKey.DownArrow)   return new Vector2Int( 0, -1);
            else return new Vector2Int(0, 0);
        }
    }
}
