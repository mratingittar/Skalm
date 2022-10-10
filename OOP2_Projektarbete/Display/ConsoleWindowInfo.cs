using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Display
{
    internal class ConsoleWindowInfo : IWindowInfo
    {
        public int WindowWidth => Console.WindowWidth;
        public int WindowHeight => Console.WindowHeight;
        public (int, int) CursorPosition => (Console.CursorLeft, Console.CursorTop);

        public void SetWindowSize(int width, int height)
        {
            try
            {
                if (width > Console.LargestWindowWidth || height > Console.LargestWindowHeight)
                    throw new ArgumentOutOfRangeException("Game window does not fit inside monitor.");

                Console.SetWindowSize(width, height);
                Console.SetBufferSize(width, height);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
            }
        }
    }
}
