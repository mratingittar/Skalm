using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Interfaces;
using Skalm.Input;
using Skalm.Map;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.States
{
    internal class PlayerStateInteract : IPlayerState
    {
        private Player _player;
        private DisplayManager _displayManager;
        private MapManager _mapManager;
        private List<BaseTile> _playerNeighbors;
        private BaseTile? _selectedTile;
        private Direction _selectedDirection;

        public PlayerStateInteract(Player player, DisplayManager displayManager, MapManager mapManager)
        {
            _player = player;
            _playerNeighbors = new List<BaseTile>();
            _displayManager = displayManager;
            _mapManager = mapManager;
        }
        public void Enter()
        {
            ExamineSameTile();
            _playerNeighbors = _mapManager.GetNeighbours(_player.GridPosition);
        }

        public void Exit()
        {
            _playerNeighbors.Clear();
            _selectedTile = null;
            _displayManager.ClearMessageSection();
        }

        public void MoveInput(Vector2Int direction)
        {
            ExamineNeighbor(direction);
        }


        public void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    if (_selectedTile != null)
                    {
                        _player.InteractWithNeighbor(_selectedTile);
                        _player.PlayerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
                    }
                    break;
                case InputCommands.Cancel:
                    _player.PlayerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
                    break;
                case InputCommands.Interact:
                    ExamineSameTile();
                    break;
                case InputCommands.Inventory:
                    break;
            }
        }

        private void ExamineSameTile()
        {
            _mapManager.TileGrid.TryGetGridObject(_player.GridPosition, out _selectedTile);
            PrintSelectedTile($"You are standing on {_selectedTile.Label}.");
        }

        private void ExamineNeighbor(Vector2Int direction)
        {
            if (direction.Equals(Vector2Int.Up))
                _selectedDirection = Direction.North;
            else if (direction.Equals(Vector2Int.Right))
                _selectedDirection = Direction.East;
            else if (direction.Equals(Vector2Int.Down))
                _selectedDirection = Direction.South;
            else if (direction.Equals(Vector2Int.Left))
                _selectedDirection = Direction.West;

            _selectedTile = _playerNeighbors.Find(n => n.GridPosition.Equals(_player.GridPosition.Add(direction)));
            if (_selectedTile != null)
            {
                PrintSelectedTile($"Looking {_selectedDirection.ToString().ToLower()} at {_selectedTile.Label}.");
            }
        }

        private void PrintSelectedTile(string baseText)
        {
            if (_selectedTile is IOccupiable occupiable && occupiable.ObjectsOnTile.Count > 0)
            {
                if (occupiable.ObjectsOnTile.Count == 1 && occupiable.ObjectsOnTile.Peek() is Player)
                    baseText += " You are alone here.";
                else
                {
                    baseText += " Tile contains:";
                    foreach (var obj in occupiable.ObjectsOnTile)
                    {
                        if (obj is not Player)                        
                            baseText += $" {obj.Label},";
                    }
                    baseText = baseText.Remove(baseText.Length - 1);
                    baseText += ".";
                }
            }
            _displayManager.DisplayInstantMessage(baseText);
        }
    }
}
