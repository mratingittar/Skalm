using Skalm.Display;
using Skalm.Input;
using Skalm.Menu;
using Skalm.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal abstract class GameStateBase : IState
    {
        protected GameManager gameManager;
        protected DisplayManager displayManager;
        protected MenuManager menuManager;
        protected SoundManager soundManager;
        protected InputManager inputManager;
        protected SceneManager sceneManager;

        public GameStateBase(GameManager gameManager)
        {
            this.gameManager = gameManager;
            this.displayManager = gameManager.DisplayManager;
            this.menuManager = gameManager.MenuManager;
            this.soundManager = gameManager.SoundManager;
            this.inputManager = gameManager.InputManager;
            sceneManager = gameManager.SceneManager;
        }

        public abstract void Enter();
        public abstract void Exit();

        public abstract void UpdateLogic();
        public abstract void UpdateDisplay();
    }
}
