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
        private readonly DisplayManager displayManager;

        public GameStatePlaying(DisplayManager displayManager)
        {
            this.displayManager = displayManager;
        }

        public void Enter()
        {
            displayManager.printer.PrintCenteredInWindow("Starting new game", displayManager.WindowHeight / 2);
            Thread.Sleep(500);
            displayManager.eraser.EraseAll();
            displayManager.DisplayHUD();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

    }
}
