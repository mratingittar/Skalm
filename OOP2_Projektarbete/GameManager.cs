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

        private List<char> animationTest;
        private int animationFrame;
        #endregion

        public GameManager()
        {
            mapManager = new MapManager(32, 32, Vector2Int.Zero);
            displayManager = new DisplayManager(new ConsoleWindowPrinter(ConsoleColor.White, ConsoleColor.Black), new ConsoleWindowEraser(), new ConsoleWindowInfo());

            GameState = new GameStateInitializing(displayManager);
            GameState.Enter();

            soundManager = new SoundManager(new ConsoleSoundPlayer(Globals.G_SOUNDS_FOLDER_PATH));

            inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());
            menuManager = new MenuManager(inputManager, displayManager, soundManager);
            menuManager.mainMenu.onMenuExecution += MainMenuExecution;

            animationTest = new List<char> { ' ', '░', '▒', '▓', '█', '▓', '▒', '░'};
            animationFrame = 0;
        }



        public void Start()
        {
            ChangeGameState(new GameStateMainMenu(displayManager, menuManager));
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
        private void MainMenuExecution(string menuPage, string item)
        {
            if (GameState is not GameStateMainMenu)
                return;

            switch (menuPage)
            {
                case "MAIN MENU":
                    //if (item == "Continue")
                    //    Continue();
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
                    soundManager.player.Play(soundManager.Tracks.Find(sound => sound.soundName == item));
                    break;
                case "INPUT METHOD":
                    inputManager.SetInputMethod(inputManager.Inputs.Find(input => input.GetType().Name == item)!);
                    break;
            }
        }
    }
}