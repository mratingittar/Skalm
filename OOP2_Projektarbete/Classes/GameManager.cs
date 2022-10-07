using OOP2_Projektarbete.Classes.Input;
using OOP2_Projektarbete.Classes.Managers;
using OOP2_Projektarbete.Classes.Map;
using OOP2_Projektarbete.Classes.States;
using OOP2_Projektarbete.Classes.Structs;

namespace OOP2_Projektarbete.Classes
{
    internal class GameManager
    {
        public IGameState GameState;
        private MainMenu mainMenu;
        private InputManager inputManager;
        private int updateFrequency;

        //public DisplayManagerGameWindow displayManagerGameWindow;
        public MapManager mapManager;
        public DisplayManager displayManager;

        public GameManager(IGameState initialState)
        {
            GameState = initialState;
            GameState.Enter();
            mapManager = new MapManager(32, 32, Vector2Int.Zero);
            displayManager = new(mapManager);

            updateFrequency = Globals.G_UPDATE_FREQUENCY;
            inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());
            mainMenu = new MainMenu(inputManager);
            mainMenu.onMenuSelection += MainMenuSelection;

            //displayManagerGameWindow = new DisplayManagerGameWindow(displayManager.gameWindowBounds, mapManager);
        }


        public void Start()
        {
            ChangeGameState(new GameStateMainMenu(mainMenu));
            Update();
            //Console.ReadKey();
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
                    ChangeGameState(new GameStatePlaying(displayManager));
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