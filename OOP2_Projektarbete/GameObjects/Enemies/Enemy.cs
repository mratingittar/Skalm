using Skalm.GameObjects.Interfaces;
using Skalm.Map;
using Skalm.States;
using Skalm.Structs;
using System.ComponentModel.DataAnnotations;

namespace Skalm.GameObjects.Enemies
{
    internal class Enemy : Actor, IDamageable
    {
        private SceneManager sceneManager;
        private MapManager mapManager;
        private EnemyStateMachine stateMachine;
        private IMoveBehaviour moveBehaviour;
        private IAttackComponent attackBehaviour;
        public Enemy(MapManager mapManager, SceneManager sceneManager, IMoveBehaviour moveBehaviour, IAttackComponent attackBehaviour, Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
            this.mapManager = mapManager;
            this.sceneManager = sceneManager;
            this.moveBehaviour = moveBehaviour;
            this.attackBehaviour = attackBehaviour;
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

            if (newPosition.Equals(GridPosition))
                return;

            if (!mapManager.TileGrid.TryGetGridObject(newPosition, out var tile))
                return;

            if (tile is ICollider collider && collider.ColliderIsActive)
            {
                collider.OnCollision();
                return;
            }

            if (tile is IOccupiable occupiable && occupiable.ActorPresent)
            {
                var obj = occupiable.ObjectsOnTile.Where(o => o is IDamageable).FirstOrDefault() as IDamageable;
                obj?.ReceiveDamage(attackBehaviour.Attack());

                return;
            }


            ExecuteMove(newPosition, GridPosition);
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

        public void ReceiveDamage(DoDamage damage)
        {

        }
    }
    internal enum EnemyMoveBehaviours
    {
        Idle,
        Random,
        Pathfinding
    }
}

