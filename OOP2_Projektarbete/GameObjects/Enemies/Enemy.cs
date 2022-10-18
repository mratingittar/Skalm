using Skalm.GameObjects.Interfaces;
using Skalm.Map;
using Skalm.States;
using Skalm.Structs;

namespace Skalm.GameObjects.Enemies
{
    internal class Enemy : Actor
    {
        private SceneManager _sceneManager;
        private MapManager _mapManager;
        private EnemyStateMachine stateMachine;
        private IMoveBehaviour moveBehaviour;
        public Enemy(MapManager mapManager, SceneManager sceneManager, IMoveBehaviour moveBehaviour, Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
            _mapManager = mapManager;
            _sceneManager = sceneManager;
            this.moveBehaviour = moveBehaviour;
            stateMachine = new EnemyStateMachine(this, EnemyStates.EnemyStateIdle);
            Player.playerTurn += MoveEnemy;
        }

        public void MoveEnemy()
        {
            base.Move(moveBehaviour.MoveDirection(GridPosition));

        }

        public void SetMoveBehaviour(EnemyMoveBehaviours behaviour)
        {
            switch (behaviour)
            {
                case EnemyMoveBehaviours.Idle:
                    moveBehaviour = new MoveIdle();
                    break;
                case EnemyMoveBehaviours.Random:
                    moveBehaviour = new MoveRandom();
                    break;
                case EnemyMoveBehaviours.Pathfinding:
                    moveBehaviour = new MovePathfinding(_mapManager, _sceneManager);
                    break;
            }
        }
    }
    internal enum EnemyMoveBehaviours
    {
        Idle,
        Random,
        Pathfinding
    }
}

