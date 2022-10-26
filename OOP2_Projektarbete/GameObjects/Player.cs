using Skalm.Display;
using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
using Skalm.Maps;
using Skalm.Maps.Tiles;
using Skalm.States;
using Skalm.Structs;

namespace Skalm.GameObjects
{
    internal class Player : Actor
    {
        internal PlayerStateMachine PlayerStateMachine { get => _playerStateMachine; }
        public EquipmentManager EquipmentManager { get => _equipmentManager; }

        // EVENTS
        public static event Action? OnPlayerTurn;
        public static event Action<ActorStatsObject, int>? OnPlayerStatsUpdated;
        public static event Action<EquipmentManager>? OnPlayerEquipmentUpdated;
        public static event Action<EquipmentManager>? OnPlayerInventoryUpdated;

        private PlayerStateMachine _playerStateMachine;
        private EquipmentManager _equipmentManager;
        private int _currentFloor;


        // CONSTRUCTOR I
        public Player(MapManager mapManager, DisplayManager displayManager, IAttackComponent attack, ActorStatsObject statsObject, string name, Vector2Int gridPosition, char sprite = '@', ConsoleColor color = ConsoleColor.White) 
            : base(mapManager, attack, statsObject, gridPosition, sprite, color)
        {
            _playerStateMachine = new PlayerStateMachine(this, displayManager, mapManager, PlayerStates.PlayerStateIdle);

            // INVENTORY
            _equipmentManager = new EquipmentManager();
            _equipmentManager.inventory.OnInventoryChanged += UpdateInventoryDisplay;
            statsObject.OnStatsChanged += UpdateStatDisplay;
        }

        // INITIALIZE PLAYER
        public void InitializePlayer(Vector2Int gridPosition, string playerName, char sprite, ConsoleColor color)
        {
            SetPlayerPosition(gridPosition);
            _sprite = sprite;
            _color = color;

            statsObject.ResetHP();
            statsObject.name = playerName;
            _equipmentManager.ResetInventory();

            _currentFloor = 0;
        }

        public void SetPlayerPosition(Vector2Int gridPosition) => GridPosition = gridPosition;
        public void NextFloor() => _currentFloor++;

        // SEND STATS TO DISPLAY
        public void UpdateAllDisplays()
        {
            UpdateStatDisplay();
            UpdateEquipmentDisplay();
            UpdateInventoryDisplay();
        }

        // UPDATE STATS DISPLAY
        public void UpdateStatDisplay()
        {
            OnPlayerStatsUpdated?.Invoke(statsObject, _currentFloor);
        }

        // UPDATE EQUIPMENT DISPLAY
        public void UpdateEquipmentDisplay()
        {
            OnPlayerEquipmentUpdated?.Invoke(_equipmentManager);
        }

        // UPDATE INVENTORY DISPLAY
        public void UpdateInventoryDisplay()
        {
            OnPlayerInventoryUpdated?.Invoke(_equipmentManager);
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
                _mapManager.MapPrinter.DrawSingleTile(neighbor.GridPosition);
                OnPlayerTurn?.Invoke();
            }
        }
    }
}
