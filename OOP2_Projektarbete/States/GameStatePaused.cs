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
    internal class GameStatePaused : GameStateBase
    {

        #region StateMachine Basics

        // CONSTRUCTOR I
        public GameStatePaused(GameManager gameManager) : base(gameManager) { }

        // STATE ENTER
        public override void Enter()
        {
            menuManager.LoadMenu(menuManager.pauseMenu);

            // INPUT EVENT SUBSCRIBE
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            // MENU EVENT SUBSCRIBE
            menuManager.pauseMenu.onMenuExecution += MenuExecution;
        }

        // STATE EXIT
        public override void Exit()
        {
            menuManager.UnloadMenu();

            // INPUT EVENT UNSUBSCRIBE
            inputManager.OnInputMove -= MoveInput;
            inputManager.OnInputCommand -= CommandInput;

            // MENU EVENT UNSUBSCRIBE
            menuManager.pauseMenu.onMenuExecution -= MenuExecution;
        }

        // STATE UPDATE LOGIC
        public override void UpdateLogic()
        {
            
        }

        // STATE UPDATE DISPLAY
        public override void UpdateDisplay()
        {
            
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
                case Page.PauseMenu:
                    if (item == "Resume")
                        gameManager.stateMachine.ChangeState(GameStates.GameStatePlaying);
                    else if (item == "Exit")
                        gameManager.stateMachine.ChangeState(GameStates.GameStateMainMenu);
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
