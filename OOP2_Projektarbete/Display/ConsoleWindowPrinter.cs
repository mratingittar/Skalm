namespace Skalm.Display
{
    internal class ConsoleWindowPrinter : IPrinter
    {
        private readonly ConsoleColor foregroundColor;
        private readonly ConsoleColor backgroundColor;

        public ConsoleWindowPrinter(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            this.foregroundColor = foregroundColor;
            this.backgroundColor = backgroundColor;
        }

        public void PrintAtPosition(char character, int y, int x)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(character);
        }

        public void PrintFromPosition(string line, int y, int x, bool highlighted = false)
        {
            if (highlighted)
            {
                Console.BackgroundColor = foregroundColor;
                Console.ForegroundColor = backgroundColor;
            }

            Console.SetCursorPosition(x, y);
            Console.Write(line);

            if (highlighted)
            {
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = foregroundColor;
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
