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
        private int updateFrequency = 10;

        public GameManager()
        {
            inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());
            mainMenu = new MainMenu(inputManager);
            mainMenu.onMenuSelection += MainMenuSelection;
            GameState = new GameStateInitializing();
        }


        public void Start()
        {
            ChangeGameState(new GameStateMainMenu(mainMenu));
            Update();
        }

        private void Update()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    inputManager.GetInput();
                }


                else if (GameState is GameStatePlaying)
                {
                    RunGame();
                }

                Thread.Sleep(1000 / updateFrequency);
            }
        }

        public void ChangeGameState(IGameState gameState)
        {
            GameState.Exit();
            GameState = gameState;
            GameState.Enter();
        }
        private void MainMenuSelection(MainMenuChoices selection)
        {
            if (GameState is not GameStateMainMenu)
                return;

            switch (selection)
            {
                case MainMenuChoices.NewGame:
                    ChangeGameState(new GameStatePlaying());
                    break;
                case MainMenuChoices.Continue:
                    break;
                case MainMenuChoices.Exit:
                    Environment.Exit(0);
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