using Skalm.Animation;
using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Enemies;
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
        public ISettings Settings { get; } 

        // MANAGERS
        public InputManager InputManager { get; }
        public SoundManager SoundManager { get; }
        public DisplayManager DisplayManager { get; }
        public MenuManager MenuManager { get; }
        public MapManager MapManager { get; }
        public SceneManager SceneManager { get; }


        // ANIMATION
        public Animator FireAnimator { get; }
        #endregion

        #region FIELDS
        private int updateFrequency;
        #endregion

        // PLAYER
        public Player? player;
        public string PlayerName { get; set; } = "";
        public bool NewGame { get; set; }
        
        // STATE MACHINES
        public readonly GameManagerStateMachine stateMachine;

        // CONSTRUCTOR I
        public GameManager(ISettings settings, DisplayManager displayManager, MapManager mapManager, SoundManager soundManager, InputManager inputManager, MenuManager menuManager, SceneManager sceneManager)
        {
            Settings = settings;

            // MANAGERS
            DisplayManager = displayManager;
            SoundManager = soundManager;
            InputManager = inputManager;
            MenuManager = menuManager;
            MapManager = mapManager;
            SceneManager = sceneManager;

            // ANIMATION
            FireAnimator = new Animator(displayManager);

            // UPDATE FREQUENCY
            updateFrequency = Settings.UpdateFrequency;


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
    }
}