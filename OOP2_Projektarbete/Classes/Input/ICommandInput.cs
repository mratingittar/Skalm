namespace OOP2_Projektarbete.Classes.Input
{
    internal interface ICommandInput
    {
        public bool GetCommandInput(ConsoleKeyInfo key, out InputCommands command);
    }
}
