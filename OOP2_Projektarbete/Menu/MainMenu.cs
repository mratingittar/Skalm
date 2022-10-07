using Skalm.Display;
using Skalm.Input;
using Skalm.Sounds;
using Skalm.Structs;
using static System.Console;

namespace Skalm.Menu
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

            SoundManager.PlayConfirmBeep();
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
            DisplayManager.PrintCenteredMultiLineText(ascii.SkalmTitle, 0);
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
                    DisplayManager.PrintCenteredText(menuNames[choice], GetCursorPosition().Top);
            }
        }

        private void PrintWithHighlight(string text)
        {
            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Black;
            DisplayManager.PrintCenteredText(text, GetCursorPosition().Top);
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
            SoundManager.PlayMoveBeep();
            PrintMenuChoices();
        }

        private void MoveMenuDown()
        {
            // Checking for highest enum value
            if ((int)menuSelection == Enum.GetValues(typeof(MainMenuChoices)).Cast<int>().Max())
                return;

            menuSelection++;
            SoundManager.PlayMoveBeep();
            PrintMenuChoices();
        }


        #endregion Menu Manipulation

    }
}
