namespace Skalm.Input
{
    internal class CommandInputKeyboard : ICommandInput
    {
        public bool GetCommandInput(ConsoleKeyInfo key, out InputCommands command)
        {
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    command = InputCommands.Confirm;
                    return true;
                case ConsoleKey.Escape:
                    command = InputCommands.Cancel;
                    return true;
                default:
                    command = InputCommands.Default;
                    return false;
            }
        }
    }
}
