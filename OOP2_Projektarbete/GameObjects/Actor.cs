using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Stats;
using Skalm.Maps;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects
{
    internal abstract class Actor : GameObject, ICollider, IDamageable
    {
        public bool ColliderIsActive { get { return true; } set { } }
        public ActorStatsObject statsObject { get; set; }
        public static event Action<Actor, Vector2Int, Vector2Int>? OnPositionChanged;
        public static event Action<string>? OnCombatEvent;

        protected IAttackComponent _attackComponent;
        protected MapManager _mapManager;

        public Actor(MapManager mapManager, IAttackComponent attackComponent, ActorStatsObject statsObject, 
            Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
            _mapManager = mapManager;
            _attackComponent = attackComponent;
            this.statsObject = statsObject;
        }

        // METHOD MOVE
        public virtual void Move(Vector2Int direction)
        {
            Vector2Int newPosition = GridPosition.Add(direction);

            if (newPosition.Equals(GridPosition))
                return;

            if (!_mapManager.TileGrid.TryGetGridObject(newPosition, out var tile))
                return;

            if (tile is ICollider collider && collider.ColliderIsActive)
            {
                collider.OnCollision();
                return;
            }

            if (tile is IOccupiable occupiable && occupiable.ActorPresent)
            {
                ExecuteAttack(occupiable);
                return;
            }

            ExecuteMove(newPosition, GridPosition);
        }

        private void ExecuteAttack(IOccupiable occupiable)
        {
            IDamageable? obj = occupiable.ObjectsOnTile.Where(o => o is IDamageable).FirstOrDefault() as IDamageable;
            if (obj != null)
            {
                string msg = _attackComponent.Attack(statsObject, obj.statsObject);
                OnCombatEvent?.Invoke(msg);
            }
        }

        protected void ExecuteMove(Vector2Int newPosition, Vector2Int oldPosition)
        {
            GridPosition = newPosition;
            OnPositionChanged?.Invoke(this, newPosition, oldPosition);
        }

        // METHOD COLLISION
        public void OnCollision()
        {

        }

        // METHOD UPDATE
        public virtual void UpdateMain()
        {

        }

        public void TakeDamage(DoDamage damage)
        {
            statsObject.TakeDamage(damage);
        }

      
    }
}
