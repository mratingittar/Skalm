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
    internal class Player : Actor
    {
        // STATE MACHINE
        public readonly PlayerStateMachine playerStateMachine;
        public readonly MapManager mapManager;
        public EquipmentManager equipmentManager;

        // MOVEMENT
        //private Vector2Int previousPosition;

        // EVENTS
        public static event Action? playerTurn;
        public static event Action<ActorStatsObject>? OnPlayerStatsUpdated;
        public static event Action<EquipmentManager>? OnPlayerInventoryUpdated;

        // CONSTRUCTOR I
        public Player(MapManager mapManager, Vector2Int posXY, IAttackComponent attack, ActorStatsObject statsObject, string name, char sprite = '@', ConsoleColor color = ConsoleColor.White) 
            : base(mapManager, attack, statsObject, posXY, sprite, color)
        {
            this.mapManager = mapManager;
            playerStateMachine = new PlayerStateMachine(this, PlayerStates.PlayerStateIdle);

            // INVENTORY
            equipmentManager = new EquipmentManager();

            // MOVEMENT      
            //previousPosition = GridPosition;

            equipmentManager.inventory.onInventoryChanged += UpdateInventoryDisplay;
            statsObject.OnStatsChanged += UpdateStatDisplay;
        }


        // INITIALIZE PLAYER
        public void InitializePlayer(Vector2Int gridPosition, string playerName, char sprite, ConsoleColor color)
        {
            GridPosition = gridPosition;            
            //previousPosition = gridPosition;
            
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
        public override void Move(Vector2Int direction)
        {
            base.Move(direction);
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
    }
}
