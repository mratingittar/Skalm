using Skalm.Actors.Fighting;
using Skalm.Actors.Spawning;
using Skalm.Actors.Stats;
using Skalm.Actors.Tile;
using Skalm.Grid;
using Skalm.Input;
using Skalm.States;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors
{
    internal class Player : Actor, IDamageable
    {
        // STATE MACHINE
        public readonly PlayerStateMachine playerStateMachine;

        // COMPONENTS
        public IAttackComponent _attack { get; set; }

        public List<Item> inventory;

        // STATS
        private StatsObjectHard statsHard;
        private StatsObjectSoft statsSoft;
        
        private Queue<Vector2Int> moveQueue;

        // CONSTRUCTOR I
        public Player(GameManager gameManager, Vector2Int posXY, IAttackComponent attack, char sprite = '@', ConsoleColor color = ConsoleColor.White) : base(posXY, sprite, color)
        {
            playerStateMachine = new PlayerStateMachine(gameManager, this, PlayerStates.PlayerStateIdle);

            // COMPONENTS
            //this._moveInput = moveInput;
            this._attack = attack;

            // STATS
            this.statsHard = new StatsObjectHard("name", 5, 5, 5, 5, 5);
            this.statsSoft = new StatsObjectSoft(10, 1);

            moveQueue = new Queue<Vector2Int>();
            inventory = new List<Item>();
        }

        // MOVE INPUT, QUEUED FOR UPDATEMAIN
        public override void Move(Vector2Int direction)
        {
            moveQueue.Enqueue(direction);
        }


        // UPDATE OBJECT
        public override void UpdateMain()
        {
            if (moveQueue.Count > 0)
            base.Move(moveQueue.Dequeue());            
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
