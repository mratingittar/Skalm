using Skalm.Structs;

namespace Skalm.Input
{
    internal class InputManager
    {
        public IMoveInput MoveInput { get; private set; }
        public ICommandInput CommandInput { get; private set; }
        public readonly List<IMoveInput> Inputs;
        public event Action<Vector2Int>? OnInputMove;
        public event Action<InputCommands>? OnInputCommand;

        public InputManager(IMoveInput moveInput, ICommandInput commandInput, List<IMoveInput> inputs)
        {
            MoveInput = moveInput;
            CommandInput = commandInput;
            Inputs = inputs;
        }

        public void SetInputMethod(IMoveInput moveInput)
        {
            MoveInput = moveInput;
        }
        public void SetInputMethod(ICommandInput commandInput)
        {
            CommandInput = commandInput;
        }

        public void GetInput()
        {
            if (!Console.KeyAvailable)
                return;

            ConsoleKeyInfo key = Console.ReadKey(true);

            if (MoveInput.GetMoveInput(key, out Vector2Int direction))
                OnInputMove?.Invoke(direction);

            if (CommandInput.GetCommandInput(key, out InputCommands command))
                OnInputCommand?.Invoke(command);

            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }
    }
}
