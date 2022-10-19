using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
using Skalm.Map;
using Skalm.States;
using Skalm.Structs;


namespace Skalm.GameObjects
{
    internal class Player : Actor
    {
        // STATE MACHINE
        public readonly PlayerStateMachine playerStateMachine;
        private MapManager mapManager;

        // COMPONENTS
        public IAttackComponent _attack { get; set; }

        public List<Item> inventory;

        // STATS
        private StatsObjectHard statsHard;
        private StatsObjectSoft statsSoft;

        private Vector2Int previousPosition;
        private Queue<Vector2Int> moveQueue;
        public static event Action? playerTurn;
        public static event Action<StatsObjectHard, StatsObjectSoft>? playerStats;
        public static event Action? playerInventory;

        // CONSTRUCTOR I
        public Player(MapManager mapManager, Vector2Int posXY, IAttackComponent attack, string name, char sprite = '@', ConsoleColor color = ConsoleColor.White) : base(posXY, sprite, color)
        {
            this.mapManager = mapManager;
            playerStateMachine = new PlayerStateMachine(this, PlayerStates.PlayerStateIdle);

            // COMPONENTS
            //this._moveInput = moveInput;
            _attack = attack;

            // STATS
            statsHard = new StatsObjectHard(name, 5, 5, 5, 5, 5);
            statsSoft = new StatsObjectSoft(10, 1);

            moveQueue = new Queue<Vector2Int>();
            inventory = new List<Item>();
            previousPosition = GridPosition;
        }

        public void InitializePlayer(Vector2Int gridPosition, string playerName, char sprite, ConsoleColor color)
        {
            GridPosition = gridPosition;
            previousPosition = gridPosition;
            statsHard.Name = playerName;
            _sprite = sprite;
            _color = color;
        }

        public void SendStatsToDisplay()
        {
            playerStats?.Invoke(statsHard, statsSoft);
            playerInventory?.Invoke();
        }

        // MOVE METHOD
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
                            damageable.ReceiveDamage(_attack.Attack());
                        }
                    }
                }
                else
                {
                    ExecuteMove(newPosition, GridPosition);
                    playerTurn?.Invoke();
                }
            }
        }

        public void InteractWithNeighbours()
        {
            foreach (var neighbour in mapManager.GetNeighbours(GridPosition))
            {
                if (neighbour is IInteractable)
                {
                    ((IInteractable)neighbour).Interact();
                    mapManager.mapPrinter.DrawSingleTile(neighbour.GridPosition);
                }
            }
        }

        // UPDATE OBJECT
        public override void UpdateMain()
        {
            //if (moveQueue.Count > 0)
            //{
            //    base.Move(moveQueue.Dequeue());
            //}

            //if (previousPosition.Equals(GridPosition) is false)
            //{

            //    previousPosition = GridPosition;
            //}

        }


        //METHOD ON COLLISION
        public void OnCollision()
        {

        }

        // METHOD RECEIVE DAMAGE
        public void ReceiveDamage(DoDamage damage)
        {

        }
    }
}
