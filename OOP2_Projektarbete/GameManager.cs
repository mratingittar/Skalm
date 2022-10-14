
using Skalm.Actors;
using Skalm.Animation;
using Skalm.Display;
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

        #region PROPERTIES
        // MANAGERS
        public InputManager InputManager { get; private set; }
        public SoundManager SoundManager { get; private set; }
        public DisplayManager DisplayManager { get; private set; }
        public MenuManager MenuManager { get; private set; }

        // MAP MANAGERS

        public MapManager MapManager { get; private set; }
        public MapPrinter MapPrinter { get; private set; }

        public ISettings Settings { get; private set; } 
        #endregion

        #region FIELDS
        private int updateFrequency;


        // ANIMATION
        private Animator animator;

        #endregion

        // PLAYER
        Player player;

        // GAME MANAGER STATE MACHINE
        public GameManagerStateMachine stateMachine;
        public List<IGameState> gameStates;

        // CONSTRUCTOR I
        public GameManager(ISettings settings, DisplayManager displayManager, MapManager mapManager, SoundManager soundManager, InputManager inputManager, MenuManager menuManager)
        {
            Settings = settings;

            // MANAGERS
            this.DisplayManager = displayManager;
            this.SoundManager = soundManager;
            this.InputManager = inputManager;
            this.MenuManager = menuManager;



            this.mapManager = mapManager;
            //this.mapPrinter = new MapPrinter(mapManager, displayManager.printer, settings.ForegroundColor);


            // UPDATE FREQUENCY
            updateFrequency = Settings.UpdateFrequency;

            // STATE MACHINE & GAME STATES
            gameStates = new List<IGameState>
            {
                new GameStateInitializing(this),
                new GameStateMainMenu(this),
                new GameStatePaused(this),
                new GameStatePlaying(this)
            };

            stateMachine = new GameManagerStateMachine(gameStates.Find(state => state is GameStateInitializing)!);


            //mapManager = new MapManager(new Grid2D<BaseTile>(displayManager.gridMapRect.Width, displayManager.gridMapRect.Height, 2, 1, displayManager.pixelGridController.cellsInSections["MapSection"].First().planePositions.First(), (x,y, gridPosition) => new VoidTile(new Vector2Int(x, y))), displayManager);

            //soundManager = new SoundManager(new ConsoleSoundPlayer(Globals.G_SOUNDS_FOLDER_PATH));

            //inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());


            // INPUT
            //inputManager.OnInputMove += MoveInput;
            //inputManager.OnInputCommand += CommandInput;

            // MENU MANAGER
            //menuManager.mainMenu.onMenuExecution += MenuExecution;
            //menuManager.pauseMenu.onMenuExecution += MenuExecution;

            // ANIMATION
            animator = new Animator(displayManager);


            // PLAYER
            var tileGrid = mapManager.TileGrid;
            Vector2Int midXY = new Vector2Int(tileGrid.gridWidth / 2, tileGrid.gridHeight / 2);
            player = new Player(mapManager.TileGrid, midXY, new PlayerMoveInput(), new PlayerAttackComponent());
            mapManager.actors.Add(player);
        }

        // METHOD UPDATE GAME
        private void Update()
        {
            while (true)
            {
                InputManager.GetInput();

                stateMachine.CurrentState.UpdateLogic();
                stateMachine.CurrentState.UpdateDisplay();

                Thread.Sleep(1000 / updateFrequency);
            }
        }

        // METHOD START STATE
        public void Start()
        {
            stateMachine.Initialize(gameStates.Find(state => state is GameStateInitializing)!);
            stateMachine.CurrentState.Enter();
            stateMachine.ChangeState(gameStates.Find(state => state is GameStateMainMenu)!);
            //ChangeGameState(gameStates.Find(state => state is GameStateMainMenu)!);
            Update();
        }


        // METHOD UPDATE GAME
        private void Update()
        {
            while (true)
            {
                inputManager.GetInput();
                if (GameState is GameStateMainMenu)
                    animator.AnimatedBraziers();

                if (GameState is GameStatePlaying)
                    mapManager.mapPrinter.RedrawCachedTiles();
                Thread.Sleep(1000 / updateFrequency);
            }
        }


        // METHOD CHANGE STATE
        //public void ChangeGameState(IGameState gameState)
        //{
        //    GameState.Exit();
        //    GameState = gameState;
        //    GameState.Enter();
        //}

        // METHOD MOVE INPUT

        //private void MoveInput(Vector2Int direction)
        //{
        //    if (GameState is GameStateMainMenu or GameStatePaused)
        //    {
        //        menuManager.TraverseMenu(direction);
        //    }
        //    else if (GameState is GameStatePlaying)
        //    {
                //player.Move(direction);
                //mapPrinter.RedrawMap();
        //    }
        //}


        // METHOD COMMAND INPUT
        //private void CommandInput(InputCommands command)
        //{
        //    if (GameState is GameStateMainMenu or GameStatePaused)
        //    {
        //        menuManager.ExecuteMenu(command);
        //    }
        //    else if (GameState is GameStatePlaying)
        //    {
        //        if (command == InputCommands.Cancel)
        //            ChangeGameState(gameStates.Find(state => state is GameStatePaused)!);
        //    }
        //}

        // METHOD EXECUTE MENU INDEX

        //private void MenuExecution(Page menuPage, string item)
        //{
        //    if (GameState is not GameStateMainMenu and not GameStatePaused)
        //        return;

        //    switch (menuPage)
        //    {
        //        case Page.MainMenu:
        //            if (item == "Exit")
        //                Environment.Exit(0);
        //            break;

        //        case Page.NewGame:
        //            if (item == "Start New Game")
        //                ChangeGameState(gameStates.Find(state => state is GameStatePlaying)!);
        //            break;

        //        case Page.Options:
        //            if (item == "Toggle Beep")
        //                soundManager.player.SFXEnabled = !soundManager.player.SFXEnabled;
        //            break;

        //        case Page.Music:
        //            soundManager.PlayMusic(soundManager.Tracks.Find(sound => sound.soundName == item));
        //            menuManager.ActiveMenu.ReloadPage();
        //            break;

        //        case Page.InputMethod:
        //            inputManager.SetInputMethod(inputManager.Inputs.Find(input => input.GetType().Name == item)!);
        //            break;

        //        case Page.PauseMenu:
        //            if (item == "Resume")
        //                ChangeGameState(gameStates.Find(state => state is GameStatePlaying)!);
        //            else if (item == "Exit")
        //                ChangeGameState(gameStates.Find(state => state is GameStateMainMenu)!);
        //            break;
        //    }
        //}

    }
}