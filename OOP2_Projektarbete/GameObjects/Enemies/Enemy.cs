using Skalm.GameObjects.Interfaces;
using Skalm.Map;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;


namespace Skalm.GameObjects.Enemies
{
    internal class Enemy : Actor
    {
        private SceneManager _sceneManager;
        private MapManager _mapManager;
        private IMoveBehaviour moveBehaviour;
        public Enemy(MapManager mapManager, SceneManager sceneManager, IMoveBehaviour moveBehaviour, Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
            _mapManager = mapManager;
            _sceneManager = sceneManager;
            this.moveBehaviour = moveBehaviour;
            Player.playerTurn += MoveEnemy;
        }

        public void MoveEnemy()
        {
                base.Move(moveBehaviour.MoveDirection(GridPosition));
        }

        public void SetMoveBehaviour(IMoveBehaviour behaviour)
        {
            moveBehaviour = behaviour;
        }
    }
}

