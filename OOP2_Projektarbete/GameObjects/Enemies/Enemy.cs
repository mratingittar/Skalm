using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Stats;
using Skalm.Map;
using Skalm.States;
using Skalm.Structs;

namespace Skalm.GameObjects.Enemies
{
    internal class Enemy : Actor
    {
        // MANAGERS
        private SceneManager _sceneManager;
        private MapManager _mapManager;
        private EnemyStateMachine stateMachine;

        // COMPONENTS
        private IMoveBehaviour moveBehaviour;

        // STATS
        //public ActorStatsObject statsObject;

        // CONSTRUCTOR I
        public Enemy(MapManager mapManager, SceneManager sceneManager, IMoveBehaviour moveBehaviour, Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
            _mapManager = mapManager;
            _sceneManager = sceneManager;
            this.moveBehaviour = moveBehaviour;
            stateMachine = new EnemyStateMachine(this, EnemyStates.EnemyStateIdle);

            //statsObject = new ActorStatsObject(new StatsObject(5, 5, 5, 5, 5, 10, 1, 0), )

            Player.playerTurn += MoveEnemy;
        }

        // MOVEMENT
        public void MoveEnemy()
        {
            base.Move(moveBehaviour.MoveDirection(GridPosition));
        }

        // SET BEHAVIOUR
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

    // ENUM ENEMY BEHAVIOURS
    internal enum EnemyMoveBehaviours
    {
        Idle,
        Random,
        Pathfinding
    }
}

