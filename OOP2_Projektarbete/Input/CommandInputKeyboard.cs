namespace Skalm.Input
{
    internal class CommandInputKeyboard : ICommandInput
    {
        public bool GetCommandInput(ConsoleKeyInfo key, out InputCommands command)
        {
            bool input;
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    command = InputCommands.Confirm;
                    input = true;
                    break;
                case ConsoleKey.Escape:
                    command = InputCommands.Cancel;
                    input = true;
                    break;
                case ConsoleKey.E:
                    command = InputCommands.Interact;
                    input = true;
                    break;
                case ConsoleKey.I:
                    command = InputCommands.Inventory;
                    input = true;
                    break;
                default:
                    command = InputCommands.Default;
                    input = false;
                    break;
            }
            return input;
        }
    }
}
