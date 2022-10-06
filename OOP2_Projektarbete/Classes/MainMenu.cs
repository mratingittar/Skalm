using OOP2_Projektarbete.Classes.Input;
using OOP2_Projektarbete.Classes.Managers;
using OOP2_Projektarbete.Classes.Structs;
using static System.Console;

namespace OOP2_Projektarbete.Classes
{
    internal partial class MainMenu
    {
        public bool Enabled { get; set; }
        private AsciiArt ascii;
        private MainMenuChoices menuSelection;
        private Dictionary<MainMenuChoices, string> menuNames;
        private int menuChoiceRowStart;
        private InputManager inputManager;
        public event Action<MainMenuChoices>? onMenuSelection;

        public MainMenu(InputManager inputManager)
        {
            this.inputManager = inputManager;
            menuSelection = MainMenuChoices.NewGame;
            menuNames = new Dictionary<MainMenuChoices, string>
            {
                [MainMenuChoices.NewGame] = "New Game",
                [MainMenuChoices.Continue] = "Continue",
                [MainMenuChoices.Exit] = "Exit"
            };
            ascii = new AsciiArt();
            inputManager.onInputMove += RecieveInput;
            inputManager.onInputCommand += RecieveInput;
            Enabled = true;
        }

        private void RecieveInput(Vector2Int dir)
        {
            if (!Enabled)
                return;

            switch (dir.Y)
            {
                case < 0:
                    MoveMenuDown();
                    break;
                case > 0:
                    MoveMenuUp();
                    break;
            }
        }

        private void RecieveInput(InputCommands command)
        {
            if (!Enabled)
                return;

            switch (command)
            {
                case InputCommands.Confirm:
                    onMenuSelection?.Invoke(menuSelection);
                    break;
                case InputCommands.Cancel:
                    Environment.Exit(0);
                    break;
            }
        }

        #region Menu Printing
        public void LoadMenu()
        {
            Clear();
            ascii.PrintFromPlace(3,0, ascii.SkalmTitle);
            WriteLine();
            WriteLine();
            menuChoiceRowStart = CursorTop;
            PrintMenuChoices();
        }

        private void PrintMenuChoices()
        {
            CursorTop = menuChoiceRowStart;
            WriteLine();
            foreach (MainMenuChoices choice in Enum.GetValues(typeof(MainMenuChoices)))
            {
                if (choice == menuSelection)
                    PrintWithHighlight(menuNames[choice]);
                else
                    WriteLine(menuNames[choice]);
            }
        }

        private void PrintWithHighlight(string text)
        {
            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Black;
            WriteLine(text);
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
        }

        #endregion Menu Printing
        #region Menu Manipulation

        private void MoveMenuUp()
        {
            // Checking for lowest enum value
            if ((int)menuSelection == Enum.GetValues(typeof(MainMenuChoices)).Cast<int>().Min())
                return;

            menuSelection--;
            PrintMenuChoices();
        }

        private void MoveMenuDown()
        {
            // Checking for highest enum value
            if ((int)menuSelection == Enum.GetValues(typeof(MainMenuChoices)).Cast<int>().Max())
                return;

            menuSelection++;
            PrintMenuChoices();
        }

        #endregion Menu Manipulation
    }
}
