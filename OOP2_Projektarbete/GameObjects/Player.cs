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
        private ActorStatsObject statsObject;

        private Vector2Int previousPosition;
        private Queue<Vector2Int> moveQueue;
        public static event Action? playerTurn;

        // CONSTRUCTOR I
        public Player(MapManager mapManager, Vector2Int posXY, IAttackComponent attack, string name, char sprite = '@', ConsoleColor color = ConsoleColor.White) : base(posXY, sprite, color)
        {
            this.mapManager = mapManager;
            playerStateMachine = new PlayerStateMachine(this, PlayerStates.PlayerStateIdle);

            // COMPONENTS
            //this._moveInput = moveInput;
            _attack = attack;

            // STATS
            statsObject = new ActorStatsObject(new StatsObject(5, 5, 5, 5, 5, 10, 1, 0), name);

            moveQueue = new Queue<Vector2Int>();
            inventory = new List<Item>();
            previousPosition = GridPosition;
        }

        public void InitializePlayer(Vector2Int gridPosition, string playerName, char sprite, ConsoleColor color)
        {
            GridPosition = gridPosition;
            statsObject.name = playerName;
            _sprite = sprite;
            _color = color;
        }

        // MOVE INPUT, QUEUED FOR UPDATEMAIN
        public override void Move(Vector2Int direction)
        {
            moveQueue.Enqueue(direction);

        }

        public void SendStatsToDisplay()
        {
            //gameManager.DisplayManager.pixelGridController.DisplayStats(statsHard, statsSoft);
            //gameManager.DisplayManager.pixelGridController.DisplayInventory();
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
            if (moveQueue.Count > 0)
            {
                base.Move(moveQueue.Dequeue());
            }

            if (previousPosition.Equals(GridPosition) is false)
            {
                playerTurn?.Invoke();
                previousPosition = GridPosition;
            }

        }

        // MOVE METHOD
        //public void Move(Vector2Int target)
        //{
        //    // CHECK GRID FOR COLLISION
        //    if (gameGrid.GetGridObject(target.X, target.Y) is ICollidable collidable)
        //    {
        //        // COLLIDE
        //        collidable.OnCollision();

        //        // IF CAN BE FOUGHT, DEAL DAMAGE
        //        if (collidable is IDamageable damageable)
        //        {
        //            damageable.ReceiveDamage(_attack.Attack());
        //        }
        //    } else {
        //        // CELL IS FREE = MAKE MOVE
        //        //tile.GridPosition = target;
        //    }
        //}

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
