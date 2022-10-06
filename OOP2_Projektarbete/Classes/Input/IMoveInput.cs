using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes.Input
{
    internal interface IMoveInput
    {
        public bool GetMoveInput(ConsoleKeyInfo key, out Vector2Int direction);
    }
}
