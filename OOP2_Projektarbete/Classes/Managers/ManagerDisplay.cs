using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.Managers
{
    internal class ManagerDisplay
    {
        // METHOD INITIALIZE WINDOW
        public void InitWindow()
        {

        }

        // METHOD BORDER PRINTER
        public void BorderPrinter(Vector2Int topleft, Vector2Int bottomright)
        {
            for (int j = topleft.Y; j < bottomright.Y; j++)
            {
                for (int i = topleft.X; i < bottomright.X; i++)
                {
                    if ((j == topleft.Y)
                    || (j == bottomright.Y)
                    || (i == topleft.X)
                    || (i == bottomright.X))
                        PrintAtPosition(i, j, Globals.G_BORDER);
                }
            }
        }

        // METHOD SET WINDOW SIZE
        public void SetWindowSize()
        {


        }

        // METHOD PRINT AT POSITION
        public void PrintAtPosition(int x, int y, char chr)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(chr);
        }
    }
}
