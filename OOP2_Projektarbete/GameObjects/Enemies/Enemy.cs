using Skalm.GameObjects.Interfaces;
using Skalm.Map;
using Skalm.States;
using Skalm.Structs;

namespace Skalm.GameObjects.Enemies
{
    internal class Enemy : Actor
    {
        private SceneManager sceneManager;
        private MapManager mapManager;
        private EnemyStateMachine stateMachine;
        private IMoveBehaviour moveBehaviour;
        public Enemy(MapManager mapManager, SceneManager sceneManager, IMoveBehaviour moveBehaviour, Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
            this.mapManager = mapManager;
            this.sceneManager = sceneManager;
            this.moveBehaviour = moveBehaviour;
            stateMachine = new EnemyStateMachine(this, EnemyStates.EnemyStateIdle);
            Player.playerTurn += MoveEnemy;
            stateMachine.ChangeState(EnemyStates.EnemyStateSearching);
        }

        public void MoveEnemy()
        {
            Move(moveBehaviour.MoveDirection(GridPosition));
        }

        public override void Move(Vector2Int direction)
        {
            Vector2Int newPosition = GridPosition.Add(direction);

            // CHECK GRID FOR COLLISION
            if (mapManager.TileGrid.TryGetGridObject(newPosition, out var tile))
            {
                if (tile is ICollider collider && collider.ColliderIsActive)
                    collider.OnCollision();

                else if (tile is IOccupiable occupiable && occupiable.ActorPresent)
                {
                    foreach (var obj in occupiable.ObjectsOnTile)
                    {
                        if (obj is IDamageable damageable)
                        {
                            //damageable.ReceiveDamage(_attack.Attack());
                        }
                    }
                }
                else
                {
                    ExecuteMove(newPosition, GridPosition);
                }
            }
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
                    moveBehaviour = new MovePathfinding(mapManager, sceneManager);
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

