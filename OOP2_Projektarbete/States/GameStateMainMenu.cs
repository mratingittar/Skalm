using Skalm.Display;
using Skalm.Input;
using Skalm.Menu;
using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.States
{
    internal class GameStateMainMenu : IGameState
    {
        public GameManager GameManager { get; }

        private MenuManager menuManager;
        private SoundManager soundManager;
        private InputManager inputManager;

        // CONSTRUCTOR I
        public GameStateMainMenu(GameManager gameManager)
        {
            this.GameManager = gameManager;

            menuManager = GameManager.MenuManager;
            soundManager = GameManager.SoundManager;
            inputManager = GameManager.InputManager;
        }

        #region StateBasics

        // ENTER STATE
        public void Enter()
        {
            menuManager.LoadMenu(menuManager.mainMenu);
            soundManager.PlayMusic(soundManager.Tracks.Find(song => song.soundName == "Video Dungeon Crawl"));

            // INPUT EVENT SUBSCRIBE
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            // MENU EVENT SUBSCRIBE
            menuManager.mainMenu.onMenuExecution += MenuExecution;
            menuManager.pauseMenu.onMenuExecution += MenuExecution;
        }

        // EXIT STATE
        public void Exit()
        {
            menuManager.UnloadMenu();

            // INPUT EVENT UNSUBSCRIBE
            inputManager.OnInputMove -= MoveInput;
            inputManager.OnInputCommand -= CommandInput;

            // MENU EVENT UNSUBSCRIBE
            menuManager.mainMenu.onMenuExecution -= MenuExecution;
            menuManager.pauseMenu.onMenuExecution -= MenuExecution;
        }

        // UPDATE LOGIC
        public void UpdateLogic()
        {

        }

        // UPDATE DISPLAY
        public void UpdateDisplay()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        // METHOD MOVE INPUT
        private void MoveInput(Vector2Int direction)
        {
            menuManager.TraverseMenu(direction);
        }

        // METHOD COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            menuManager.ExecuteMenu(command);
        }

        // METHOD EXECUTE MENU INDEX
        private void MenuExecution(Page menuPage, string item)
        {
            switch (menuPage)
            {
                case Page.MainMenu:
                    if (item == "Exit")
                        Environment.Exit(0);
                    break;

                case Page.NewGame:
                    if (item == "Start New Game")
                        GameManager.stateMachine.ChangeState(GameManager.gameStates.Find(state => state is GameStatePlaying)!);
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
            }
        }
        #endregion
    }
}
