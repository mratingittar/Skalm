using static System.Console;


namespace OOP2_Projektarbete
{
    internal class MainMenu
    {
        private string skalm = @"
   _______  _        __   __  _        _______ 
  (  ____ \| \    /\(__) (__)( \      (       )
  | (    \/|  \  / / _______ | (      | () () |
  | (_____ |  (_/ / (  ___  )| |      | || || |
  (_____  )|   _ (  | (___) || |      | |(_)| |
        ) ||  ( \ \ |  ___  || |      | |   | |
  /\____) ||  /  \ \| )   ( || (____/\| )   ( |
  \_______)|_/    \/|/     \|(_______/|/     \|
                                               ";

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

            LoadMenu();
            MenuSelection();
        }

        private void LoadMenu()
        {
            Clear();
            WriteLine(skalm);
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

        private enum MenuChoices
        {
            NewGame,
            Continue,
            Exit
        }

        private void MenuSelection() // To be refactored into using input system
        {
            while (true)
            {
                ConsoleKey key = ReadKey().Key;

                if (key == ConsoleKey.Escape)
                    ExitGame();
                else if (key == ConsoleKey.UpArrow)
                    MoveMenuUp();
                else if (key == ConsoleKey.DownArrow)
                    MoveMenuDown();
                else if (key == ConsoleKey.Enter)
                    ConfirmSelection();
            }
        }

        private void MoveMenuUp()
        {
            // Checking for lowest enum value
            if ((int)menuSelection == Enum.GetValues(typeof(MenuChoices)).Cast<int>().Min())
                return;

            menuSelection--;
            PrintMenuChoices();
        }

        private void MoveMenuDown()
        {
            // Checking for highest enum value
            if ((int)menuSelection == Enum.GetValues(typeof(MenuChoices)).Cast<int>().Max())
                return;

            menuSelection++;
            PrintMenuChoices();
        }

        private void ConfirmSelection()
        {
            if (menuSelection == MenuChoices.Exit)
                ExitGame();
        }

        private void ExitGame()
        {
            Environment.Exit(0);
        }
    }

}
