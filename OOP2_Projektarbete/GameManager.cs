using Skalm.Display;
using Skalm.Input;
using Skalm.Map;
using Skalm.Menu;
using Skalm.Sounds;
using Skalm.States;
using Skalm.Structs;

namespace Skalm
{
    internal class GameManager
    {

        #region FIELDS
        public IGameState GameState;
        private int updateFrequency = Globals.G_UPDATE_FREQUENCY;

        // MANAGERS
        private InputManager inputManager;
        private SoundManager soundManager;
        public MapManager mapManager;
        public DisplayManager displayManager;
        private MenuManager menuManager;
        #endregion

        public GameManager()
        {
            mapManager = new MapManager(32, 32, Vector2Int.Zero);
            displayManager = new DisplayManager(new ConsoleWindowPrinter(ConsoleColor.White, ConsoleColor.Black), new ConsoleWindowEraser(), new ConsoleWindowInfo());

            GameState = new GameStateInitializing(displayManager);
            GameState.Enter();

            soundManager = new SoundManager(new ConsoleSoundPlayer(Globals.G_SOUNDS_FOLDER_PATH));

            inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());
            menuManager = new MenuManager(inputManager, displayManager, soundManager.player);
            menuManager.mainMenu.onMenuExecution += MainMenuExecution;
        }



        public void Start()
        {
            ChangeGameState(new GameStateMainMenu(displayManager, menuManager));
            Update();
        }

        private void Update()
        {
            while (true)
            {
                inputManager.GetInput(); // INPUTS ARE QUEUEING UP, NEEDS FIXING.

                Thread.Sleep(1000 / updateFrequency);
            }
        }

        public void ChangeGameState(IGameState gameState)
        {
            GameState.Exit();
            GameState = gameState;
            GameState.Enter();
        }

        // MAY CHANGE THIS TO THE MENUS HAVING A REFERENCE TO THIS METHOD AND SENDING IN AN ENUM INSTEAD.
        private void MainMenuExecution(string item)
        {
            if (GameState is not GameStateMainMenu)
                return;

            switch (item)
            {
                case "Start New Game":
                    ChangeGameState(new GameStatePlaying(displayManager));
                    break;
                case "Continue":
                    break;
                case "Toggle Beep":
                    soundManager.player.SFXEnabled = !soundManager.player.SFXEnabled;
                    break;
                default:
                    break;
            }
        }
    }
}