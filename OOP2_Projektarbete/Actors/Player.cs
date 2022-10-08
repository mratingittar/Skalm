﻿using Skalm.Actors.Fighting;
using Skalm.Actors.Stats;
using Skalm.Grid;
using Skalm.Input;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors
{
    internal class Player : IGameObject, IMoveable, ICollidable, IDamageable
    {
        // POSITION
        public Grid<Cell> gameGrid;
        public Vector2Int posXY { get; private set; }

        // COMPONENTS
        public IMoveInput _moveInput { get; set; }

        // STATS
        public StatsObjectHard statsHard;
        public StatsObjectSoft statsSoft;

        // CONSTRUCTOR I
        public Player(Grid<Cell> gameGrid, Vector2Int posXY, IMoveInput moveInput)
        {
            // GAME WORLD
            this.gameGrid = gameGrid;
            this.posXY = posXY;

            // COMPONENTS
            this._moveInput = moveInput;

            // STATS
            statsHard = new StatsObjectHard(5, 5, 5, 5, 5);
            statsSoft = new StatsObjectSoft(10, 1);
        }

        // UPDATE OBJECT
        public void UpdateMain()
        {

        }

        // MOVE METHOD
        public void Move(Vector2Int target)
        {
            // CHECK GRID FOR COLLISION
            if (gameGrid.GetGridObject(target) is ICollidable collidable)
            {
                // COLLIDE
                collidable.OnCollision();

                // IF CAN BE FOUGHT, DEAL DAMAGE
                if (collidable is IDamageable damageable)
                {
                    damageable.ReceiveDamage(DoDamage());
                }
            } else {
                // CELL IS FREE = MAKE MOVE
                posXY = target;
            }
        }

        // METHOD ON COLLISION
        public void OnCollision()
        {

        }

        // METHOD DO DAMAGE
        public DoDamage DoDamage()
        {
            return new DoDamage();
        }

        // METHOD RECEIVE DAMAGE
        public void ReceiveDamage(DoDamage damage)
        {

        }
    }
}