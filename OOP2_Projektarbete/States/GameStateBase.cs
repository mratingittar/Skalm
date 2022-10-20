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
        protected GameManager _gameManager;
        protected DisplayManager _displayManager;
        protected MenuManager _menuManager;
        protected SoundManager _soundManager;
        protected InputManager _inputManager;
        protected SceneManager _sceneManager;

        public GameStateBase(GameManager gameManager)
        {
            _gameManager = gameManager;
            _displayManager = gameManager.DisplayManager;
            _menuManager = gameManager.MenuManager;
            _soundManager = gameManager.SoundManager;
            _inputManager = gameManager.InputManager;
            _sceneManager = gameManager.SceneManager;
        }

        public abstract void Enter();
        public abstract void Exit();

        public abstract void UpdateLogic();
        public abstract void UpdateDisplay();
    }
}
