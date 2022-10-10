using Skalm.Structs;

namespace Skalm.Display
{
    internal class ConsoleWindowEraser : IEraser
    {
        public void EraseArea(Bounds area)
        {
            for (int x = area.StartXY.X; x < area.EndXY.X; x++)
            {
                for (int y = area.StartXY.Y; y < area.EndXY.Y; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }
        }

        public void EraseLinesFromTo(int yStart, int yEnd)
        {
            for (int x = 0; x < Console.WindowWidth; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }
        }

        public void EraseAll()
        {
            Console.Clear();
        }
    }
}
