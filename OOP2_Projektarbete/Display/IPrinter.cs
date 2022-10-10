namespace Skalm.Display
{
    internal interface IPrinter
    {
        void PrintAtPosition(char character, int y, int x);
        void PrintFromPosition(string line, int y, int x, bool highlighted = false);
        void PrintFromPosition(string[] lines, int y, int x, bool highlighted = false);
        void PrintCenteredInWindow(string line, int y, bool highlighted = false);
        void PrintCenteredInWindow(string[] line, int y, bool highlighted = false);        
    }
}
