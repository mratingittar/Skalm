using OOP2_Projektarbete.Classes.Input;
using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes.Managers
{
    internal class InputManager
    {
        private IMoveInput moveInput;
        private ICommandInput commandInput;
        public event Action<Vector2Int>? onInputMove;
        public event Action<InputCommands>? onInputCommand;

        public InputManager(IMoveInput moveInput, ICommandInput commandInput)
        {
            this.moveInput = moveInput;
            this.commandInput = commandInput;
        }

        public void GetInput()
        {
            if (!Console.KeyAvailable)
                return;

            ConsoleKeyInfo key = Console.ReadKey(true);

            if (moveInput.GetMoveInput(key, out Vector2Int direction))
                onInputMove?.Invoke(direction);

            if (commandInput.GetCommandInput(key, out InputCommands command))
                onInputCommand?.Invoke(command);
        }
    }
}
