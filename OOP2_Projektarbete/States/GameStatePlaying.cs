using Skalm.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStatePlaying : IGameState
    {
        private readonly DisplayManager _displayManager;

        // CONSTRUCTOR I
        public GameStatePlaying(DisplayManager displayManager)
        {
            _displayManager = displayManager;
        }

        // ENTER GAME PLAYING STATE
        public void Enter()
        {
            DisplayManager.PrintCenteredText("Starting new game", Console.WindowHeight / 2);
            Thread.Sleep(500);
            Console.Clear();
            _displayManager.DisplayHUD();
        }

        // EXIT GAME PLAYING STATE
        public void Exit()
        {
            throw new NotImplementedException();
        }

        // UPDATE STATE LOGIC
        public void UpdateLogic()
        {
            // _gameManager.UpdateGameLoop/(;
        }

        // UPDATE STATE DISPLAY
        public void UpdateDisplay()
        {
            // _displayManager.UpdateDisplayWindows();
        }
    }
}
