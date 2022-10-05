using OOP2_Projektarbete.Classes.Input;
using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes.Managers
{
    internal class InputManager
    {
        private IMoveInput moveInput;
        public Action<Vector2Int>? onInputCommand;

        public InputManager(IMoveInput moveInput)
        {
            this.moveInput = moveInput;
           
        }

        public void ParseCommand()
        {
            onInputCommand?.Invoke(GetMoveDirection());
        }

        public Vector2Int GetMoveDirection()
        {
            return moveInput.GetMoveInput();
        }

    }
}
