
using Skalm.Actors;
using Skalm.Actors.Fighting;
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
        public ISettings Settings { get; private set; } 

        // MANAGERS
        public InputManager InputManager { get; private set; }
        public SoundManager SoundManager { get; private set; }
        public DisplayManager DisplayManager { get; private set; }
        public MenuManager MenuManager { get; private set; }
        public MapManager MapManager { get; private set; }


        // ANIMATION
        public Animator FireAnimator { get; private set; }
        #endregion

        #region FIELDS
        private int updateFrequency;
        #endregion

        // PLAYER
        public Player player;
        public bool NewGame { get; set; }

        // GAME MANAGER STATE MACHINE
        public GameManagerStateMachine stateMachine;
        public List<IGameState> gameStates;

        // CONSTRUCTOR I
        public GameManager(ISettings settings, DisplayManager displayManager, MapManager mapManager, SoundManager soundManager, InputManager inputManager, MenuManager menuManager)
        {
            Settings = settings;

            // MANAGERS
            DisplayManager = displayManager;
            SoundManager = soundManager;
            InputManager = inputManager;
            MenuManager = menuManager;
            MapManager = mapManager;

            // ANIMATION
            FireAnimator = new Animator(displayManager);

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

            stateMachine = new GameManagerStateMachine(gameStates.Where(state => state is GameStateInitializing).First());
            player = new Player(Vector2Int.Zero, new PlayerAttackComponent());
        }

        // METHOD START STATE
        public void Start()
        {
            stateMachine.Initialize(gameStates.Where(state => state is GameStateInitializing).First());
            stateMachine.ChangeState(gameStates.Where(state => state is GameStateMainMenu).First());
            Update();
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

        public void CreatePlayer()
        {
            var tileGrid = MapManager.TileGrid;
            Vector2Int midXY = new Vector2Int(tileGrid.gridWidth / 2, tileGrid.gridHeight / 2);
            player = new Player(midXY, new PlayerAttackComponent());
            MapManager.actors.Add(player);
            MapManager.gameObjects.Add(player);
        }
    }
}