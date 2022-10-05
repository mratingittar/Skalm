
namespace OOP2_Projektarbete.Classes
{
    internal class GameManager
    {
        public void Run()
        {
            MainMenu mainMenu = new MainMenu();
            PrintGrid();

            switch (mainMenu.Menu())
            {
                case MainMenu.MenuChoices.NewGame:
                    break;
                case MainMenu.MenuChoices.Continue:
                    break;
                case MainMenu.MenuChoices.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        private void NewGame()
        {

        }

        private void PrintGrid()
        {
            Grid<Cell> grid = new Grid<Cell>(32, 32, 2, 1, new(0, 0), (position, x, y) => new Cell(position, x, y));
            Console.Clear();
            grid.PrintGrid('*');
            Console.ReadKey();
        }

        private void ContinueGame()
        {

        }

    }
}