namespace Skalm.Input
{
    internal interface ICommandInput
    {
        public bool GetCommandInput(ConsoleKeyInfo key, out InputCommands command);
    }
}
