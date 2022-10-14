using Skalm.Input;
using Skalm.Menu;
using Skalm.Sounds;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStatePaused : IGameState
    {
        public GameManager GameManager { get; }

        private MenuManager menuManager;
        private SoundManager soundManager;
        private InputManager inputManager;

        #region StateMachine Basics

        // CONSTRUCTOR I
        public GameStatePaused(GameManager gameManager)
        {
            this.GameManager = gameManager;

            menuManager = GameManager.MenuManager;
            soundManager = GameManager.SoundManager;
            inputManager = GameManager.InputManager;
        }

        // STATE ENTER
        public void Enter()
        {
            menuManager.LoadMenu(menuManager.pauseMenu);

            // INPUT EVENT SUBSCRIBE
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            // MENU EVENT SUBSCRIBE
            menuManager.mainMenu.onMenuExecution += MenuExecution;
            menuManager.pauseMenu.onMenuExecution += MenuExecution;
        }

        // STATE EXIT
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

        // STATE UPDATE LOGIC
        public void UpdateLogic()
        {
            throw new NotImplementedException();
        }

        // STATE UPDATE DISPLAY
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

        private void MenuExecution(Page menuPage, string item)
        {
            switch (menuPage)
            {
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
                        GameManager.stateMachine.ChangeState(GameManager.gameStates.Find(state => state is GameStatePlaying)!);
                    else if (item == "Exit")
                        GameManager.stateMachine.ChangeState(GameManager.gameStates.Find(state => state is GameStateMainMenu)!);
                    break;
            }
        }

        #endregion
    }
}
