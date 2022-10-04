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
        // BOUNDS OF THE MESSAGE DISPLAY AREA
        private Vector2Int startXY;
        private Vector2Int endXY;

        // CURRENT MESSAGE STRING & DISPLAY DURATION
        private string currMsg;
        private int displayCounter;

        // CONSTRUCTOR I
        public HUDmsgBox(Vector2Int startXY, Vector2Int endXY)
        {
            this.startXY = startXY;
            this.endXY = endXY;
            currMsg = "";
            displayCounter = 0;
        }

        // METHOD UPDATE CURRENT MESSAGE
        private void UpdateMessage(string msg, int dispDur = 5)
        {
            currMsg = msg;
            displayCounter += dispDur;
        }

        // METHOD DISPLAY MESSAGE 
        public void DisplayMessage(string msg)
        {
            UpdateMessage(msg);

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
