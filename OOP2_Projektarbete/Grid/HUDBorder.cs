

namespace Skalm.Grid
{
    internal class HUDBorder : IHUD
    {
        public readonly char borderCharacter;

        public HUDBorder(char borderCharacter)
        {
            this.borderCharacter = borderCharacter;
        }
    }
}
