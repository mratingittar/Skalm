using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Stats;
using Skalm.Map;
using Skalm.States;
using Skalm.Structs;
using System.Security.Cryptography.X509Certificates;

namespace Skalm.GameObjects.Enemies
{
    internal class Enemy : Actor, IDisposable
    {
        // MANAGERS
        private SceneManager _sceneManager;
        private bool _isAlive;
        private EnemyStateMachine _stateMachine;

        // COMPONENTS
        private IMoveBehaviour moveBehaviour;

        private bool _disposed;

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

        public void Die()
        {
            _isAlive = false;
            _sceneManager.RemoveGameObject(this);
        }

        public void Remove()
        {
            Player.OnPlayerTurn -= MoveEnemy;
            statsObject.OnDeath -= Die;
            Dispose();
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

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
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

