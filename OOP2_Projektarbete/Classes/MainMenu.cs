using static System.Console;


namespace OOP2_Projektarbete.Classes
{
    internal class MainMenu
    {
        private AsciiArt ascii;
        private MenuChoices menuSelection;
        private Dictionary<MenuChoices, string> menuNames;
        private int menuChoiceRowStart;

        public MainMenu()
        {
            CursorVisible = false;
            menuSelection = MenuChoices.NewGame;
            menuNames = new Dictionary<MenuChoices, string>
            {
                [MenuChoices.NewGame] = "New Game",
                [MenuChoices.Continue] = "Continue",
                [MenuChoices.Exit] = "Exit"
            };
            ascii = new AsciiArt();
        }

        public MenuChoices Menu() 
        {
            LoadMenu();
            while (true)
            {
                ConsoleKey key = ReadKey().Key;
                
                if (key == ConsoleKey.Escape)
                    return MenuChoices.Exit;
                else if (key == ConsoleKey.UpArrow)
                {
                    MoveMenuUp();
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    MoveMenuDown();
                }
                else if (key == ConsoleKey.Enter)
                {
                    PlayBeep(880);
                    return menuSelection;
                }
            }
        }

        private void LoadMenu()
        {
            Clear();
            WriteLine(ascii.SkalmTitle);
            WriteLine();
            WriteLine();
            menuChoiceRowStart = CursorTop;
            PrintMenuChoices();
        }

        private void PrintMenuChoices()
        {
            CursorTop = menuChoiceRowStart;
            WriteLine();
            foreach (MenuChoices choice in Enum.GetValues(typeof(MenuChoices)))
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

        public enum MenuChoices
        {
            NewGame,
            Continue,
            Exit
        }

        private void MoveMenuUp()
        {
            // Checking for lowest enum value
            if ((int)menuSelection == Enum.GetValues(typeof(MenuChoices)).Cast<int>().Min())
                return;

            menuSelection--;
            PlayBeep(440);
            PrintMenuChoices();
        }

        private void MoveMenuDown()
        {
            // Checking for highest enum value
            if ((int)menuSelection == Enum.GetValues(typeof(MenuChoices)).Cast<int>().Max())
                return;

            menuSelection++;
            PlayBeep(440);
            PrintMenuChoices();
        }

        private void PlayBeep(int freq)
        {
            if (OperatingSystem.IsWindows())
                Beep(freq, 100);
        }
    }
}
