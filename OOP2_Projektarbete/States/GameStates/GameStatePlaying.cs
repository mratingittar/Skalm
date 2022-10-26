using Skalm.Display;
using Skalm.GameObjects;
using Skalm.Input;
using Skalm.Maps;
using Skalm.Sounds;
using Skalm.States.PlayerStates;
using Skalm.Structs;

namespace Skalm.States.GameStates
{
    internal class GameStatePlaying : IGameState
    {
        private GameManager _gameManager;
        private MapManager _mapManager;
        private MapPrinter _mapPrinter;
        private DisplayManager _displayManager;
        private SoundManager _soundManager;
        private InputManager _inputManager;
        private SceneManager _sceneManager;

        // CONSTRUCTOR I
        public GameStatePlaying(GameManager gameManager)
        {
            _gameManager = gameManager;
            _sceneManager = gameManager.SceneManager;
            _mapManager = gameManager.MapManager;
            _mapPrinter = _mapManager.MapPrinter;
            _displayManager = gameManager.DisplayManager;
            _soundManager = gameManager.SoundManager;
            _inputManager = gameManager.InputManager;
            _sceneManager = gameManager.SceneManager;
        }

        #region State Machine Basics

        // ENTER GAME PLAYING STATE
        public void Enter()
        {
            // INPUT EVENT SUBSCRIBE
            _inputManager.OnInputMove += MoveInput;
            _inputManager.OnInputCommand += CommandInput;


            if (_gameManager.NewGame)
            {
                _displayManager.Printer.PrintCenteredInWindow("ENTERING THE DUNGEON", _displayManager.WindowInfo.WindowHeight / 2, _gameManager.Settings.TextColor);
                Thread.Sleep(500);

                // CREATE MAP
                _mapManager.MapGenerator.CreateMap();
                _sceneManager.NewGame();
            }

            // DRAWING HUD & MAP
            _displayManager.Eraser.EraseAll();
            _displayManager.DisplayHUD();
            _mapManager.MapPrinter.DrawMap();

            _sceneManager.Player.UpdateAllDisplays();
            _sceneManager.Player.PlayerStateMachine.ChangeState(EPlayerStates.PlayerStateMove);
            _soundManager.PlayMusic(_soundManager.Tracks.Find(song => song.soundName == "Thunder Dreams"));
        }

        // EXIT GAME PLAYING STATE
        public void Exit()
        {
            // SAVING STATE
            _gameManager.NewGame = false;
            _sceneManager.Player.PlayerStateMachine.ChangeState(EPlayerStates.PlayerStateIdle);
            _displayManager.Eraser.EraseAll();

            // INPUT EVENT UNSUBSCRIBE
            _inputManager.OnInputMove -= MoveInput;
            _inputManager.OnInputCommand -= CommandInput;

            _gameManager.PlayerName = "";
        }

        // UPDATE STATE LOGIC
        public void UpdateLogic()
        {
            // NO ACTOR USES UPDATE FUNCTION!

            //foreach (Actor actor in _sceneManager.ActorsInScene)
            //{
            //    actor.UpdateMain();
            //}

            if (_sceneManager.Player.GridPosition.Equals(_mapManager.MapGenerator.CurrentMap.GoalPosition))
                _sceneManager.NextLevel();
        }

        // UPDATE STATE DISPLAY
        public void UpdateDisplay()
        {
            _mapPrinter.RedrawCachedTiles();
            if (_displayManager.MessagesInQueue > 0 && _sceneManager.Player.PlayerStateMachine.CurrentState is not PlayerStateMessage)
                _sceneManager.Player.PlayerStateMachine.ChangeState(EPlayerStates.PlayerStateMessage);
        }

        #endregion

        #region Methods

        //METHOD MOVE INPUT
        private void MoveInput(Vector2Int direction)
        {
            _soundManager.player.Play(SoundManager.SoundType.Low);
            _sceneManager.Player.PlayerStateMachine.CurrentState.MoveInput(direction);
        }

        // METHOD COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            _soundManager.player.Play(SoundManager.SoundType.Mid);
            _sceneManager.Player.PlayerStateMachine.CurrentState.CommandInput(command);
        }

        #endregion
    }
}
