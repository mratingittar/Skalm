using Skalm.Animation;
using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Enemies;
using Skalm.Input;
using Skalm.Maps;
using Skalm.Menu;
using Skalm.Sounds;
using Skalm.States.GameStates;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm
{
    internal class GameManager
    {
        #region PROPERTIES
        public ISettings Settings { get; } 
        public GameStateMachine StateMachine => _stateMachine;

        // MANAGERS
        public InputManager InputManager { get; }
        public SoundManager SoundManager { get; }
        public DisplayManager DisplayManager { get; }
        public MenuManager MenuManager { get; }
        public MapManager MapManager { get; }
        public SceneManager SceneManager { get; }

        // PLAYER
        public string PlayerName { get; set; } = "";
        public bool NewGame { get; set; }

        // ANIMATION
        public Animator Animator { get; }
        #endregion

        #region FIELDS
        private readonly int _updateFrequency;
        private readonly GameStateMachine _stateMachine;
        #endregion

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
            Animator = new Animator(displayManager, settings);

            // UPDATE FREQUENCY
            _updateFrequency = Settings.UpdateFrequency == 0 ? 20 : Settings.UpdateFrequency;

            // STATE MACHINE & GAME STATES
            _stateMachine = new GameStateMachine(this, EGameStates.GameStateInitializing);
            _stateMachine.Initialize(EGameStates.GameStateInitializing);
        }

        // HANDLE GAME OVER
        public void GameOver() => _stateMachine.ChangeState(EGameStates.GameStateGameOver);

        // METHOD START STATE
        public void Start()
        {
            _stateMachine.ChangeState(EGameStates.GameStateMainMenu);
            Update();
        }

        // METHOD UPDATE GAME
        private void Update()
        {
            while (true)
            {
                InputManager.GetInput();

                _stateMachine.CurrentState.UpdateLogic();
                _stateMachine.CurrentState.UpdateDisplay();

                Thread.Sleep(1000 / _updateFrequency);
            }
        }
    }
}