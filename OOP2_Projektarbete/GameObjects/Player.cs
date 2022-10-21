using Skalm.Display;
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

        // EVENTS
        public static event Action? OnPlayerTurn;
        public static event Action<ActorStatsObject, int>? OnPlayerStatsUpdated;
        public static event Action<EquipmentManager>? OnPlayerInventoryUpdated;

        private int _currentFloor;

        // CONSTRUCTOR I
        public Player(MapManager mapManager, DisplayManager displayManager, IAttackComponent attack, ActorStatsObject statsObject, string name, Vector2Int gridPosition, char sprite = '@', ConsoleColor color = ConsoleColor.White) 
            : base(mapManager, attack, statsObject, gridPosition, sprite, color)
        {
            this.mapManager = mapManager;
            playerStateMachine = new PlayerStateMachine(this, displayManager, PlayerStates.PlayerStateIdle);

            // INVENTORY
            equipmentManager = new EquipmentManager();

            equipmentManager.inventory.onInventoryChanged += UpdateInventoryDisplay;
            statsObject.OnStatsChanged += UpdateStatDisplay;
        }

        // INITIALIZE PLAYER
        public void InitializePlayer(Vector2Int gridPosition, string playerName, char sprite, ConsoleColor color)
        {
            SetPlayerPosition(gridPosition);
            _sprite = sprite;
            _color = color;

            statsObject.ResetHP();
            this.statsObject.name = playerName;
            equipmentManager.ResetInventory();

            _currentFloor = 1;
        }

        public void SetPlayerPosition(Vector2Int gridPosition) => GridPosition = gridPosition;
        public void NextFloor() => _currentFloor++;

        // SEND STATS TO DISPLAY
        public void SendStatsToDisplay()
        {
            UpdateStatDisplay();
            UpdateInventoryDisplay();
        }

        // UPDATE STATS DISPLAY
        private void UpdateStatDisplay()
        {
            OnPlayerStatsUpdated?.Invoke(statsObject, _currentFloor);
        }

        // UPDATE INVENTORY DISPLAY
        private void UpdateInventoryDisplay()
        {
            OnPlayerInventoryUpdated?.Invoke(equipmentManager);
        }

        // MOVE METHOD
        public override void Move(Vector2Int direction)
        {
            base.Move(direction);
            OnPlayerTurn?.Invoke();
        }

        // INTERACT WITH NEIGHBOURS
        public void InteractWithNeighbor(BaseTile neighbor)
        {
            // CHECK FOR OCCUPIABLE TILE
            if ((neighbor is IOccupiable occupiable)
                && (occupiable.ObjectsOnTile.Count > 0))
            {
                // IF PICKUP ITEM, PICK IT UP
                foreach (var item in occupiable.ObjectsOnTile)
                {
                    if (item is ItemPickup i)
                    {
                        i.Interact(this);
                        break;
                    }
                }
                OnPlayerTurn?.Invoke();
                return;
            }

            // CHECK FOR INTERACTABLE TILE
            if (neighbor is IInteractable interactable)
            {
                interactable.Interact(this);
                mapManager.mapPrinter.DrawSingleTile(neighbor.GridPosition);
                OnPlayerTurn?.Invoke();
            }
        }
    }
}
