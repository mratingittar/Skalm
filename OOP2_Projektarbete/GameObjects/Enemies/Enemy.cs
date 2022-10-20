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
        private bool _isAlive;
        private EnemyStateMachine _stateMachine;

        // COMPONENTS
        private IMoveBehaviour moveBehaviour;

        // CONSTRUCTOR I
        public Enemy(MapManager mapManager, SceneManager sceneManager, IMoveBehaviour moveBehaviour, IAttackComponent attackBehaviour, ActorStatsObject statsObject, 
            Vector2Int gridPosition, char sprite, ConsoleColor color) : base(mapManager, attackBehaviour, statsObject, gridPosition, sprite, color)
        {
            _isAlive = true;
            _mapManager = mapManager;
            _sceneManager = sceneManager;
            this.moveBehaviour = moveBehaviour;
            _stateMachine = new EnemyStateMachine(this, EnemyStates.EnemyStateIdle);

            Player.OnPlayerTurn += MoveEnemy;
            statsObject.OnDeath += Die;
            _stateMachine.ChangeState(EnemyStates.EnemyStateSearching);
        }

        private void Die()
        {
            _isAlive = false;
            _sceneManager.RemoveGameObject(this);
            _mapManager.mapPrinter.CacheUpdatedTile(GridPosition);
        }

        // MOVEMENT
        public void MoveEnemy()
        {
            if (_isAlive)
                Move(moveBehaviour.MoveDirection(GridPosition));
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

