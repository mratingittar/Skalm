using System.Drawing;

namespace Skalm.Display
{
    internal class ConsoleWindowPrinter : IPrinter
    {
        public void PrintAtPosition(char character, int y, int x, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(character);
        }

        public void PrintFromPosition(string line, int y, int x, ConsoleColor color = ConsoleColor.White, bool highlighted = false)
        {
            Console.ForegroundColor = color;
            ConsoleColor bg = Console.BackgroundColor;

            if (highlighted)
            {
                Console.BackgroundColor = color;
                Console.ForegroundColor = bg;
            }

            Console.SetCursorPosition(x, y);
            Console.Write(line);

            if (highlighted)
            {
                Console.BackgroundColor = bg;
                Console.ForegroundColor = color;
            }
        }

        public void PrintFromPosition(string[] lines, int y, int x, ConsoleColor color = ConsoleColor.White, bool highlighted = false)
        {
            for (int i = 0; i < lines.Length; i++)
                PrintFromPosition(lines[i], y + i, x, color, highlighted);
        }

        public void PrintCenteredInWindow(string line, int y, ConsoleColor color = ConsoleColor.White, bool highlighted = false)
        {
            PrintFromPosition(line, y, FindOffsetFromConsoleCenter(line.Length), color, highlighted);

        }

        public void PrintCenteredInWindow(string[] lines, int y, ConsoleColor color = ConsoleColor.White, bool highlighted = false)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                PrintCenteredInWindow(lines[i], y + i, color, highlighted);
            }
        }

        private int FindOffsetFromConsoleCenter(int width)
        {
            return (int)((float)Console.WindowWidth / 2 - (float)width / 2);
        }

    }
}
