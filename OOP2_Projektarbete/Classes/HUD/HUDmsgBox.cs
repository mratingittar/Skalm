using OOP2_Projektarbete.Classes.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2_Projektarbete.Classes.HUD
{
    internal class HUDmsgBox
    {
        private Vector2Int startXY;
        private Vector2Int endXY;

        // CONSTRUCTOR I
        public HUDmsgBox(Vector2Int startXY, Vector2Int endXY)
        {
            this.startXY = startXY;
            this.endXY = endXY;
        }

        // METHOD DISPLAY MESSAGE 
        public void DisplayMessage(string msg)
        {
            int counter = 0;
            for (int j = startXY.Y; j < endXY.Y; j++)
            {
                for (int i = startXY.X; i < endXY.X; i++)
                {
                    Console.SetCursorPosition(i, j);
                    if (counter < msg.Length) Console.Write(msg[counter]);
                    else Console.Write(' ');
                    counter++;
                }
            }
        }
    }
}
