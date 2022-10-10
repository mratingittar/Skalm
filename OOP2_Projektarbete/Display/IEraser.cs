using Skalm.Structs;

namespace Skalm.Display
{
    internal interface IEraser
    {
        public void EraseArea(Bounds area);
        public void EraseLinesFromTo(int yStart, int yEnd);
        public void EraseAll();
    }
}
