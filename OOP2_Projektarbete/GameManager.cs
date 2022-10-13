using Skalm.Display;
using Skalm.Grid;
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
        private List<IGameState> gameStates;
        private int updateFrequency;

        // MANAGERS
        private InputManager inputManager;
        private SoundManager soundManager;
        private MapManager mapManager;
        private DisplayManager displayManager;
        private MenuManager menuManager;


        private List<char> animationTest;
        private int animationFrame;
        #endregion
        public ISettings Settings { get; private set; }

        public GameManager(ISettings settings, DisplayManager displayManager, MapManager mapManager, SoundManager soundManager, InputManager inputManager, MenuManager menuManager)
        {
            Settings = settings;
            this.displayManager = displayManager;
            this.mapManager = mapManager;
            this.soundManager = soundManager;
            this.inputManager = inputManager;
            this.menuManager = menuManager;

            updateFrequency = Settings.UpdateFrequency;

            gameStates = new List<IGameState>
            {
                new GameStateInitializing(displayManager),
                new GameStateMainMenu(menuManager),
                new GameStatePaused(menuManager),
                new GameStatePlaying(displayManager)
            };
            GameState = gameStates.Where(state => state is GameStateInitializing).First();
            GameState.Enter();

 
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            menuManager.mainMenu.onMenuExecution += MenuExecution;
            menuManager.pauseMenu.onMenuExecution += MenuExecution;


            animationTest = new List<char> { ' ', '░', '▒', '▓', '█', '▓', '▒', '░' };
            animationFrame = 0;
        }



        public void Start()
        {
            ChangeGameState(gameStates.Find(state => state is GameStateMainMenu)!);
            soundManager.PlayMusic(soundManager.Tracks.Find(song => song.soundName == "Video Dungeon Crawl"));
            Update();
        }

        private void Animate()
        {
            if (animationFrame == animationTest.Count)
                animationFrame = 0;

            displayManager.printer.PrintAtPosition(animationTest[animationFrame], 10, 10);
            animationFrame++;
        }

        private void Update()
        {
            while (true)
            {

                inputManager.GetInput();


                Thread.Sleep(1000 / updateFrequency);
            }
        }

        public void ChangeGameState(IGameState gameState)
        {
            GameState.Exit();
            GameState = gameState;
            GameState.Enter();
        }
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

        private void MenuExecution(string menuPage, string item)
        {
            if (GameState is not GameStateMainMenu and not GameStatePaused)
                return;

            switch (menuPage)
            {
                case "MAIN MENU":
                    if (item == "Exit")
                        Environment.Exit(0);
                    break;
                case "NEW GAME":
                    if (item == "Start New Game")
                        ChangeGameState(new GameStatePlaying(displayManager));
                    break;
                case "OPTIONS":
                    if (item == "Toggle Beep")
                        soundManager.player.SFXEnabled = !soundManager.player.SFXEnabled;
                    break;
                case "MUSIC":
                    soundManager.PlayMusic(soundManager.Tracks.Find(sound => sound.soundName == item));
                    menuManager.ActiveMenu.ReloadPage();
                    break;
                case "INPUT METHOD":
                    inputManager.SetInputMethod(inputManager.Inputs.Find(input => input.GetType().Name == item)!);
                    break;
                case "PAUSE MENU":
                    if (item == "Resume")
                        ChangeGameState(gameStates.Find(state => state is GameStatePlaying)!);
                    else if (item == "Exit")
                        ChangeGameState(gameStates.Find(state => state is GameStateMainMenu)!);
                    break;
            }
        }
    }
}