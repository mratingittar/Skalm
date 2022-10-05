
namespace OOP2_Projektarbete.Classes
{
    internal class GameManager
    {
        public void Run()
        {
            MainMenu mainMenu = new MainMenu();

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

        private void ContinueGame()
        {

        }

    }
}