using OOP2_Projektarbete.Classes.States;
using OOP2_Projektarbete.Classes.Managers;
using OOP2_Projektarbete.Classes.Input;

namespace OOP2_Projektarbete.Classes
{
    internal class GameManager
    {
        public IGameState GameState;
        private MainMenu mainMenu;
        private InputManager inputManager;

        public GameManager(IGameState initialState)
        {
            GameState = initialState;
            mainMenu = new MainMenu();
            inputManager = new InputManager(new InputKeys());
        }

        public void Run()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    inputManager.ParseCommand();
                }

                if (GameState is GameStateMainMenu)
                {
                    RunMainMenu();

                }
                else if (GameState is GameStatePlaying)
                {
                    RunGame();
                }

                Thread.Sleep(100);
            }
        }

        public void ChangeGameState(IGameState gameState)
        {
            GameState.Exit();
            GameState = gameState;
            GameState.Enter();
        }

        private void RunMainMenu()
        {
            switch (mainMenu.Menu())
            {
                case MainMenu.MenuChoices.NewGame:
                    ChangeGameState(new GameStatePlaying());
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
        private void RunGame()
        {

        }

        private void NewGame()
        {

        }

        private void ContinueGame()
        {

        }

    }
}