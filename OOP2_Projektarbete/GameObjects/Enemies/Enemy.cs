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
        public override string Label { get => statsObject.name; }
        public static event Action<Enemy>? OnEnemyDeath; 

        // MANAGERS
        private bool _isAlive;
        private EnemyStateMachine _stateMachine;

        // COMPONENTS
        private IMoveBehaviour _moveBehaviour;
        private Player _player;

        private bool _disposed;

        // CONSTRUCTOR I
        public Enemy(MapManager mapManager, Player player, IMoveBehaviour moveBehaviour, IAttackComponent attackBehaviour, ActorStatsObject statsObject, 
            Vector2Int gridPosition, char sprite, ConsoleColor color) : base(mapManager, attackBehaviour, statsObject, gridPosition, sprite, color)
        {
            _player = player;
            _isAlive = true;
            _mapManager = mapManager;
            
            _moveBehaviour = moveBehaviour;
            _stateMachine = new EnemyStateMachine(this, EnemyStates.EnemyStateIdle);

            Player.OnPlayerTurn += MoveEnemy;
            statsObject.OnDeath += Die;
            _stateMachine.ChangeState(EnemyStates.EnemyStateSearching);
        }

        public void Die()
        {
            _isAlive = false;
            OnEnemyDeath?.Invoke(this);
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
                Move(_moveBehaviour.MoveDirection(GridPosition));
        }

        // SET BEHAVIOUR
        public void SetMoveBehaviour(EnemyMoveBehaviours behaviour)
        {
            switch (behaviour)
            {
                case EnemyMoveBehaviours.Idle:
                    _moveBehaviour = new MoveIdle();
                    break;
                case EnemyMoveBehaviours.Random:
                    _moveBehaviour = new MoveRandom();
                    break;
                case EnemyMoveBehaviours.Pathfinding:
                    _moveBehaviour = new MovePathfinding(_mapManager, _player);
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

