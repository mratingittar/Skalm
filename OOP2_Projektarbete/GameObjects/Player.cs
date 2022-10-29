using Skalm.Display;
using Skalm.GameObjects.Enemies;
using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
using Skalm.Maps;
using Skalm.Maps.Tiles;
using Skalm.States.PlayerStates;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.GameObjects
{
    internal class Player : Actor
    {
        // PROPERTIES
        internal PlayerStateMachine PlayerStateMachine { get => _playerStateMachine; }
        public EquipmentManager EquipmentManager { get => _equipmentManager; }

        // EVENTS
        public static event Action? OnPlayerTurn;
        public static event Action<ActorStatsObject, int>? OnPlayerStatsUpdated;
        public static event Action<EquipmentManager>? OnPlayerEquipmentUpdated;
        public static event Action<EquipmentManager>? OnPlayerInventoryUpdated;

        // FIELDS
        private PlayerStateMachine _playerStateMachine;
        private EquipmentManager _equipmentManager;
        private int _currentFloor;


        // CONSTRUCTOR I
        public Player(MapManager mapManager, DisplayManager displayManager, IAttackComponent attack, ActorStatsObject statsObject, string name, Vector2Int gridPosition, char sprite = '@', ConsoleColor color = ConsoleColor.White) 
            : base(mapManager, attack, statsObject, gridPosition, sprite, color)
        {
            _playerStateMachine = new PlayerStateMachine(this, displayManager, mapManager, EPlayerStates.PlayerStateIdle);

            // INVENTORY
            _equipmentManager = new EquipmentManager();
            _equipmentManager.inventory.OnInventoryChanged += UpdateInventoryDisplay;
            statsObject.OnStatsChanged += UpdateStatDisplay;
        }

        // INITIALIZE PLAYER
        public void InitializePlayer(Vector2Int gridPosition, string playerName, char sprite, ConsoleColor color)
        {
            // SET POSITION & MAP CHARACTERISTICS
            SetPlayerPosition(gridPosition);
            _sprite = sprite;
            _color = color;

            // SET STATS OBJECT & INVENTORY
            statsObject = GeneratePlayerStats(playerName, 15);
            statsObject.ResetHP();
            statsObject.name = playerName;
            _equipmentManager.ResetInventory();

            // RESET PROGRESS
            _currentFloor = 0;
        }

        // GENERATE RANDOM PLAYER STATS OBJECT
        private ActorStatsObject GeneratePlayerStats(string name, int startingHP, int statPoints = 40, int statMinimum = 3)
        {
            // PLAYER STATS OBJECT
            StatsObject stats = new StatsObject(statMinimum, statMinimum, statMinimum, statMinimum, statMinimum, startingHP, 1, 1);

            // RANDOMIZE STATS
            int statTmp;
            statPoints -= (statMinimum * 5);
            for (int i = 0; i < statPoints; i++)
            {
                statTmp = Dice.rng.Next(0, 6);
                stats.statsArr[statTmp].AddValue(1);
            }

            // CREATE & RETURN ACTOR STATS OBJECT
            return new ActorStatsObject(stats, name, 0);
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
