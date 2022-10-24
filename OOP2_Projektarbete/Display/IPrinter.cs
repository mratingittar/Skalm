namespace Skalm.Display
{
    internal interface IPrinter
    {
        void PrintAtPosition(char character, int y, int x, ConsoleColor color = ConsoleColor.White);
        void PrintFromPosition(string line, int y, int x, ConsoleColor color = ConsoleColor.White, bool highlighted = false);
        void PrintFromPosition(string[] lines, int y, int x, ConsoleColor color = ConsoleColor.White, bool highlighted = false);
        void PrintCenteredInWindow(string line, int y, ConsoleColor color = ConsoleColor.White, bool highlighted = false);
        void PrintCenteredInWindow(string[] line, int y, ConsoleColor color = ConsoleColor.White, bool highlighted = false);        
    }
}
