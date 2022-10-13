﻿using Skalm.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStateInitializing : IGameState
    {
        public GameManager gameManager { get; }

        private readonly DisplayManager displayManager;

        // CONSTRUCTOR I
        public GameStateInitializing(GameManager gameManager)
        {
            this.gameManager = gameManager;

            displayManager = gameManager.displayManager;
        }

        // ENTER STATE
        public void Enter()
        {
            displayManager.printer.PrintCenteredInWindow("Loading SKÄLM", displayManager.windowInfo.WindowHeight / 2);
        }

        // EXIT STATE
        public void Exit()
        {
            displayManager.eraser.EraseAll(); 
            displayManager.printer.PrintCenteredInWindow("Loaded", displayManager.windowInfo.WindowHeight / 2);
            Thread.Sleep(500);
            displayManager.eraser.EraseAll();
        }

        // UPDATE LOGIC
        public void UpdateDisplay() {}

        // UPDATE DISPLAY
        public void UpdateLogic() {}
    }
}
