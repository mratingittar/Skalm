using Skalm.Input;
using Skalm.Menu;
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
        public GameManager gameManager { get; }

        private readonly MenuManager menuManager;

        #region StateMachine Basics

        // CONSTRUCTOR I
        public GameStatePaused(GameManager gameManager)
        {
            this.gameManager = gameManager;

            menuManager = gameManager.menuManager;
        }

        // STATE ENTER
        public void Enter()
        {
            menuManager.LoadMenu(menuManager.pauseMenu);
        }

        // STATE EXIT
        public void Exit()
        {
            menuManager.UnloadMenu();
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

        #endregion
    }
}
