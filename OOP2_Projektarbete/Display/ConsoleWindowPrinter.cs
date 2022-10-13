namespace Skalm.Display
{
    internal class ConsoleWindowPrinter : IPrinter
    {
        public void PrintAtPosition(char character, int y, int x)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(character);
        }

        public void PrintFromPosition(string line, int y, int x, bool highlighted = false)
        {
            ConsoleColor fg = Console.ForegroundColor;
            ConsoleColor bg = Console.BackgroundColor;

            if (highlighted)
            {
                Console.BackgroundColor = fg;
                Console.ForegroundColor = bg;
            }

            Console.SetCursorPosition(x, y);
            Console.Write(line);

            if (highlighted)
            {
                Console.BackgroundColor = bg;
                Console.ForegroundColor = fg;
            }
        }

        public void PrintFromPosition(string[] lines, int y, int x, bool highlighted = false)
        {
            for (int i = 0; i < lines.Length; i++)
                PrintFromPosition(lines[i], y + i, x, highlighted);
        }

        public void PrintCenteredInWindow(string line, int y, bool highlighted = false)
        {
            PrintFromPosition(line, y, FindOffsetFromConsoleCenter(line.Length), highlighted);

        }

        public void PrintCenteredInWindow(string[] lines, int y, bool highlighted = false)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                PrintCenteredInWindow(lines[i], y + i, highlighted);
            }
        }

        private int FindOffsetFromConsoleCenter(int width)
        {
            return (int)((float)Console.WindowWidth / 2 - (float)width / 2);
        }

    }
}
