using Skalm.Display;
using Skalm.Menu;
using Skalm.Sounds;

namespace Skalm.States
{
    internal class GameStateMainMenu : IGameState
    {
        private MenuManager menuManager;
        private SoundManager soundManager;

        // CONSTRUCTOR I
        public GameStateMainMenu(MenuManager menuManager, SoundManager soundManager)
        {
            this.menuManager = menuManager;
            this.soundManager = soundManager;
        }

        // ENTER STATE
        public void Enter()
        {
            menuManager.LoadMenu(menuManager.mainMenu);
            soundManager.PlayMusic(soundManager.Tracks.Find(song => song.soundName == "Video Dungeon Crawl"));
        }

        // EXIT STATE
        public void Exit()
        {
            menuManager.UnloadMenu();
        }

        // UPDATE LOGIC
        public void UpdateLogic()
        {
            throw new NotImplementedException();
        }

        // UPDATE DISPLAY
        public void UpdateDisplay()
        {
            throw new NotImplementedException();
        }
    }
}
