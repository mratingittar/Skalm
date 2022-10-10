using Skalm.Structs;

namespace Skalm.Input
{
    internal class InputManager
    {
        private IMoveInput moveInput;
        private ICommandInput commandInput;
        public readonly List<IMoveInput> Inputs;
        public event Action<Vector2Int>? OnInputMove;
        public event Action<InputCommands>? OnInputCommand;

        public InputManager(IMoveInput moveInput, ICommandInput commandInput)
        {
            this.moveInput = moveInput;
            this.commandInput = commandInput;
            Inputs = new List<IMoveInput>
            {
                new MoveInputArrowKeys(),
                new MoveInputWASD(),
                new MoveInputNumpad()
            };
        }

        public void SetInputMethod(IMoveInput moveInput)
        {
            this.moveInput = moveInput;
        }
        public void SetInputMethod(ICommandInput commandInput)
        {
            this.commandInput = commandInput;
        }

        public void GetInput()
        {
            if (!Console.KeyAvailable)
                return;

            ConsoleKeyInfo key = Console.ReadKey(true);

            if (moveInput.GetMoveInput(key, out Vector2Int direction))
                OnInputMove?.Invoke(direction);

            if (commandInput.GetCommandInput(key, out InputCommands command))
                OnInputCommand?.Invoke(command);

            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }
    }
}
