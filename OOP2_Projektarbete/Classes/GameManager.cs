using OOP2_Projektarbete.Classes.States;
using OOP2_Projektarbete.Classes.Managers;
using OOP2_Projektarbete.Classes.Input;
using OOP2_Projektarbete.Classes.Map;
using OOP2_Projektarbete.Classes.Structs;
using OOP2_Projektarbete.Classes.Grid;

namespace OOP2_Projektarbete.Classes
{
    internal class GameManager
    {
        public IGameState GameState;
        private MainMenu mainMenu;
        private InputManager inputManager;
        private int updateFrequency = 10;

        public WindowManagerConsole displayManager;
        public DisplayManagerGameWindow displayManagerGameWindow;
        public MapManager mapManager;

        public GameManager()
        {
            inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());
            mainMenu = new MainMenu(inputManager);
            mainMenu.onMenuSelection += MainMenuSelection;
            GameState = new GameStateInitializing();

            mapManager = new MapManager();
            displayManager = new WindowManagerConsole();
            displayManagerGameWindow = new DisplayManagerGameWindow(displayManager.gameWindowBounds, mapManager);
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
                    PrintGrid();
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

        private void PrintGrid()
        {
            Grid<Cell> grid = new Grid<Cell>(32, 32, 2, 1, new(5,15), (position, x, y) => new Cell(position, x, y));
            Console.Clear();
            grid.PrintGrid('*');
            Console.ReadKey();
        }

        private void ContinueGame()
        {

        }

    }
}