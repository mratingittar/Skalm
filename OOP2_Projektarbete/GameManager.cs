
using Skalm.Actors;
using Skalm.Actors.Fighting;
using Skalm.Actors.Spawning;
using Skalm.Actors.Tile;
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
        public string PlayerName { get; set; } = "";
        public Enemy? enemy;
        public bool NewGame { get; set; }
        
        // STATE MACHINES
        public readonly GameManagerStateMachine stateMachine;

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

            player = new Player(this, Vector2Int.Zero, new PlayerAttackComponent(), "blank");

            // STATE MACHINE & GAME STATES
            stateMachine = new GameManagerStateMachine(this, GameStates.GameStateInitializing);

            stateMachine.Initialize(GameStates.GameStateInitializing);
        }

        // METHOD START STATE
        public void Start()
        {
            stateMachine.ChangeState(GameStates.GameStateMainMenu);
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
            Vector2Int spawnPos = MapManager.mapGenerator.GetRandomSpawnPosition();
            if (PlayerName.Length == 0)
                PlayerName = "Nameless";
            player = new Player(this, spawnPos, new PlayerAttackComponent(), PlayerName, '@', ConsoleColor.Yellow);
            MapManager.actors.Add(player);
            MapManager.gameObjects.Add(player);

            enemy = new Enemy(this, MapManager.mapGenerator.GetRandomSpawnPosition(), 'Q', ConsoleColor.Red);
            MapManager.actors.Add(enemy);
        }
    }
}