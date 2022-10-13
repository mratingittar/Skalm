using Skalm.Actors;
using Skalm.Actors.Tile;
using Skalm.Display;
using Skalm.Grid;
using Skalm.Input;
using Skalm.Map;
using Skalm.Menu;
using Skalm.Sounds;
using Skalm.States;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm
{
    internal class GameManager
    {
        #region FIELDS
        public IGameState GameState;
        private List<IGameState> gameStates;
        private int updateFrequency;

        // MANAGERS
        private InputManager inputManager;
        private SoundManager soundManager;
        private DisplayManager displayManager;
        private MenuManager menuManager;

        // MAP MANAGERS
        private MapManager mapManager;
        private MapPrinter mapPrinter;

        // ANIMATION
        private List<char> animationTest;
        private int animationFrame;
        #endregion
        public ISettings Settings { get; private set; }

        // PLAYER
        Player player;

        // CONSTRUCTOR I
        public GameManager(ISettings settings, DisplayManager displayManager, MapManager mapManager, SoundManager soundManager, InputManager inputManager, MenuManager menuManager)
        {
            Settings = settings;

            // MANAGERS
            this.displayManager = displayManager;
            this.soundManager = soundManager;
            this.inputManager = inputManager;
            this.menuManager = menuManager;

            this.mapManager = mapManager;
            this.mapPrinter = new MapPrinter(mapManager, displayManager);

            // UPDATE FREQUENCY
            updateFrequency = Settings.UpdateFrequency;

            // GAME STATES
            gameStates = new List<IGameState>
            {
                new GameStateInitializing(displayManager),
                new GameStateMainMenu(menuManager, soundManager),
                new GameStatePaused(menuManager),
                new GameStatePlaying(displayManager, soundManager, mapManager)
            };

            GameState = gameStates.Where(state => state is GameStateInitializing).First();
            GameState.Enter();

            //mapManager = new MapManager(new Grid2D<BaseTile>(displayManager.gridMapRect.Width, displayManager.gridMapRect.Height, 2, 1, displayManager.pixelGridController.cellsInSections["MapSection"].First().planePositions.First(), (x,y, gridPosition) => new VoidTile(new Vector2Int(x, y))), displayManager);
            
            //soundManager = new SoundManager(new ConsoleSoundPlayer(Globals.G_SOUNDS_FOLDER_PATH));

            //inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());

            // INPUT
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            // MENU MANAGER
            menuManager.mainMenu.onMenuExecution += MenuExecution;
            menuManager.pauseMenu.onMenuExecution += MenuExecution;

            // ANIMATION
            animationTest = new List<char> { ' ', '░', '▒', '▓', '█', '▓', '▒', '░' };
            animationFrame = 0;

            // PLAYER
            var tileGrid = mapManager.tileGrid;
            Vector2Int midXY = new Vector2Int(tileGrid.gridWidth / 2, tileGrid.gridHeight / 2);
            player = new Player(mapManager.tileGrid, midXY, new PlayerMoveInput(), new PlayerAttackComponent());
            mapManager.gameObjects.Add(player);
        }

        // METHOD START STATE
        public void Start()
        {
            ChangeGameState(gameStates.Find(state => state is GameStateMainMenu)!);
            Update();
        }

        // METHOD ANIMATE
        private void Animate() 
        {
            if (animationFrame == animationTest.Count)
                animationFrame = 0;

            displayManager.printer.PrintAtPosition(animationTest[animationFrame], 10, 10);
            animationFrame++;
        }

        // METHOD UPDATE GAME
        private void Update()
        {
            while (true)
            {
                inputManager.GetInput();

                Thread.Sleep(1000 / updateFrequency);
            }
        }

        // METHOD CHANGE STATE
        public void ChangeGameState(IGameState gameState)
        {
            GameState.Exit();
            GameState = gameState;
            GameState.Enter();
        }

        // METHOD MOVE INPUT
        private void MoveInput(Vector2Int direction)
        {
            if (GameState is GameStateMainMenu or GameStatePaused)
            {
                menuManager.TraverseMenu(direction);
            }
            else if (GameState is GameStatePlaying)
            {

            }
        }

        // METHOD COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            if (GameState is GameStateMainMenu or GameStatePaused)
            {
                menuManager.ExecuteMenu(command);
            }
            else if (GameState is GameStatePlaying)
            {
                if (command == InputCommands.Cancel)
                    ChangeGameState(gameStates.Find(state => state is GameStatePaused)!);
            }
        }

        // METHOD EXECUTE MENU INDEX
        private void MenuExecution(Page menuPage, string item)
        {
            if (GameState is not GameStateMainMenu and not GameStatePaused)
                return;

            switch (menuPage)
            {
                case Page.MainMenu:
                    if (item == "Exit")
                        Environment.Exit(0);
                    break;

                case Page.NewGame:
                    if (item == "Start New Game")
                        ChangeGameState(gameStates.Find(state => state is GameStatePlaying)!);
                    break;

                case Page.Options:
                    if (item == "Toggle Beep")
                        soundManager.player.SFXEnabled = !soundManager.player.SFXEnabled;
                    break;

                case Page.Music:
                    soundManager.PlayMusic(soundManager.Tracks.Find(sound => sound.soundName == item));
                    menuManager.ActiveMenu.ReloadPage();
                    break;

                case Page.InputMethod:
                    inputManager.SetInputMethod(inputManager.Inputs.Find(input => input.GetType().Name == item)!);
                    break;

                case Page.PauseMenu:
                    if (item == "Resume")
                        ChangeGameState(gameStates.Find(state => state is GameStatePlaying)!);
                    else if (item == "Exit")
                        ChangeGameState(gameStates.Find(state => state is GameStateMainMenu)!);
                    break;
            }
        }
    }
}