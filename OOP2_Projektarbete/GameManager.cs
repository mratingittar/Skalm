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
        private MainMenu mainMenu;
        private int updateFrequency = Globals.G_UPDATE_FREQUENCY;

        // MANAGERS
        private InputManager inputManager;
        private SoundManager soundManager;
        public MapManager mapManager;
        public DisplayManager displayManager; 
        #endregion

        public GameManager(IGameState initialState)
        {
            GameState = initialState;
            GameState.Enter();

            soundManager = new SoundManager();
            soundManager.PlayMusic(soundManager.defaultSound);
            //soundManager.PlayMusic(soundManager.Sounds.Find(sound => sound.soundName == "Thunder Dreams"));

            mapManager = new MapManager(32, 32, Vector2Int.Zero);
            displayManager = new(mapManager);
            inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());
            mainMenu = new MainMenu(inputManager);
            mainMenu.onMenuSelection += MainMenuSelection;
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
                    ChangeGameState(new GameStatePlaying(displayManager));
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


        private void ContinueGame()
        {

        }
    }
}