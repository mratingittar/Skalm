using Skalm.Display;
using Skalm.Sounds;
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
        private readonly SoundManager soundManager;

        // CONSTRUCTOR I
        public GameStatePlaying(DisplayManager displayManager, SoundManager soundManager)
        {
            this.displayManager = displayManager;
            this.soundManager = soundManager;
        }

        // ENTER GAME PLAYING STATE
        public void Enter()
        {
            // IF NEW GAME
            // ELSE IF RESUMING


            displayManager.printer.PrintCenteredInWindow("Starting new game", displayManager.windowInfo.WindowHeight / 2);
            Thread.Sleep(500);
            displayManager.eraser.EraseAll();
            displayManager.DisplayHUD();
            soundManager.PlayMusic(soundManager.Tracks.Find(song => song.soundName == "Thunder Dreams"));
            displayManager.pixelGridController.DisplayMessage("Welcome to the Land of Skälm.");
        }

        // EXIT GAME PLAYING STATE
        public void Exit()
        {
            // SAVING STATE
            displayManager.eraser.EraseAll();

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
