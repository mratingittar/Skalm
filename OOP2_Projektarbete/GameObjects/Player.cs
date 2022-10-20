using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
using Skalm.Map;
using Skalm.Map.Tile;
using Skalm.States;
using Skalm.Structs;
using System.Reflection.Emit;


namespace Skalm.GameObjects
{
    internal class Player : Actor, IDamageable
    {
        // STATE MACHINE
        public readonly PlayerStateMachine playerStateMachine;
        public readonly MapManager mapManager;

        // COMPONENTS
        public IAttackComponent _attackComponent { get; set; }

        // STATS
        public ActorStatsObject statsObject { get; set; }
        public EquipmentManager equipmentManager;

        // MOVEMENT
        private Vector2Int previousPosition;
        private Queue<Vector2Int> moveQueue;
        public static event Action? playerTurn;
        public static event Action<ActorStatsObject>? OnPlayerStatsUpdated;
        public static event Action<EquipmentManager>? OnPlayerInventoryUpdated;

        // CONSTRUCTOR I
        public Player(MapManager mapManager, Vector2Int posXY, IAttackComponent attack, string name, char sprite = '@', ConsoleColor color = ConsoleColor.White) 
            : base(posXY, sprite, color)
        {
            this.mapManager = mapManager;
            playerStateMachine = new PlayerStateMachine(this, PlayerStates.PlayerStateIdle);

            // COMPONENTS
            //this._moveInput = moveInput;
            _attackComponent = attack;

            // STATS
            statsObject = new ActorStatsObject(new StatsObject(5, 5, 5, 5, 5, 10, 1, 0), name);
            equipmentManager = new EquipmentManager();

            // MOVEMENT
            moveQueue = new Queue<Vector2Int>();            
            previousPosition = GridPosition;

            equipmentManager.inventory.onInventoryChanged += UpdateInventoryDisplay;
            statsObject.OnStatsChanged += UpdateStatDisplay;
        }


        // INITIALIZE PLAYER
        public void InitializePlayer(Vector2Int gridPosition, string playerName, char sprite, ConsoleColor color)
        {
            GridPosition = gridPosition;            
            previousPosition = gridPosition;
            
            _sprite = sprite;
            _color = color;
            
            statsObject.name = playerName;
            
        }

        // SEND STATS TO DISPLAY
        public void SendStatsToDisplay()
        {
            UpdateStatDisplay();
            UpdateInventoryDisplay();
        }

        private void UpdateStatDisplay()
        {
            OnPlayerStatsUpdated?.Invoke(statsObject);
        }

        private void UpdateInventoryDisplay()
        {
            OnPlayerInventoryUpdated?.Invoke(equipmentManager);
        }

        // MOVE METHOD
        public override void Move(Vector2Int direction) // MOVE PARTS TO ACTOR CLASS, COMBINE WITH ENEMY MOVE. DRY!
        {
            Vector2Int newPosition = GridPosition.Add(direction);

            if (!mapManager.TileGrid.TryGetGridObject(newPosition, out var tile))
                return;

            if (tile is ICollider collider && collider.ColliderIsActive)
            {
                collider.OnCollision();
                return;
            }

            if (tile is IOccupiable occupiable && occupiable.ActorPresent)
            {
                IDamageable? obj = occupiable.ObjectsOnTile.Where(o => o is IDamageable).FirstOrDefault() as IDamageable;
                if (obj != null)
                    _attackComponent.Attack(statsObject, obj.statsObject);
                playerTurn?.Invoke();
                return;
            }

            ExecuteMove(newPosition, GridPosition);
            playerTurn?.Invoke();
        }

        // INTERACT WITH NEIGHBOURS
        public void InteractWithNeighbor(BaseTile neighbor)
        {
            if ((neighbor is IOccupiable occupiable)
                && (occupiable.ObjectsOnTile.Count > 0))
            {
                //occupiable.ObjectsOnTile.ForEach(x => x is ItemPickup )
                foreach (var item in occupiable.ObjectsOnTile)
                {
                    if (item is ItemPickup i)
                        i.Interact(this);
                }

                return;
            }

            if (neighbor is IInteractable interactable)
            {
                interactable.Interact(this);
                mapManager.mapPrinter.DrawSingleTile(neighbor.GridPosition);
            }
        }

        // UPDATE OBJECT
        public override void UpdateMain()
        {


        }

        //METHOD ON COLLISION
        public void OnCollision()
        {

        }

        // METHOD RECEIVE DAMAGE
        public void TakeDamage(DoDamage damage)
        {

        }
    }
}
