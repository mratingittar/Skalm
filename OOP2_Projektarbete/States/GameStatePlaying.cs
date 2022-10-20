using Skalm.Display;
using Skalm.GameObjects;
using Skalm.Input;
using Skalm.Map;

using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.States
{
    internal class GameStatePlaying : GameStateBase
    {
        private SceneManager _sceneManager;
        private MapManager mapManager;
        private MapPrinter mapPrinter;

        // CONSTRUCTOR I
        public GameStatePlaying(GameManager gameManager) : base(gameManager)
        {
            _sceneManager = gameManager.SceneManager;
            mapManager = gameManager.MapManager;
            mapPrinter = mapManager.mapPrinter;
        }

        #region State Machine Basics

        // ENTER GAME PLAYING STATE
        public override void Enter()
        {
            // INPUT EVENT SUBSCRIBE
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;
            

            if (gameManager.NewGame)
            {
                displayManager.Printer.PrintCenteredInWindow("ENTERING SKÄLM", displayManager.WindowInfo.WindowHeight / 2);
                Thread.Sleep(500);
                


                // CREATE MAP
                mapManager.mapGenerator.CreateMap();
                _sceneManager.InitializeScene();
            }

            // DRAWING HUD & MAP
            displayManager.Eraser.EraseAll();
            displayManager.DisplayHUD();
            mapManager.mapPrinter.DrawMap();
           
            _sceneManager.Player.SendStatsToDisplay();
            _sceneManager.Player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
            soundManager.PlayMusic(soundManager.Tracks.Find(song => song.soundName == "Thunder Dreams"));
        }

        // EXIT GAME PLAYING STATE
        public override void Exit()
        {
            // SAVING STATE
            gameManager.NewGame = false;
            _sceneManager.Player.playerStateMachine.ChangeState(PlayerStates.PlayerStateIdle);
            displayManager.Eraser.EraseAll();

            // INPUT EVENT UNSUBSCRIBE
            inputManager.OnInputMove -= MoveInput;
            inputManager.OnInputCommand -= CommandInput;

            gameManager.PlayerName = "";
        }

        // UPDATE STATE LOGIC
        public override void UpdateLogic()
        {
            foreach (Actor actor in _sceneManager.ActorsInScene)
            {
                actor.UpdateMain();
            }
        }

        // UPDATE STATE DISPLAY
        public override void UpdateDisplay()
        {
            mapPrinter.RedrawCachedTiles();
            if (displayManager.MessagesInQueue > 0 && _sceneManager.Player.playerStateMachine.CurrentState is not PlayerStateMessage)
                _sceneManager.Player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMessage);
        }

        #endregion

        #region Methods

        //METHOD MOVE INPUT
        private void MoveInput(Vector2Int direction)
        {
            soundManager.player.Play(SoundManager.SoundType.Low);
            _sceneManager.Player.playerStateMachine.CurrentState.MoveInput(direction);
        }

        // METHOD COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            soundManager.player.Play(SoundManager.SoundType.Mid);
            _sceneManager.Player.playerStateMachine.CurrentState.CommandInput(command);
        }

        #endregion
    }
}
