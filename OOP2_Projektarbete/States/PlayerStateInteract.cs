using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Interfaces;
using Skalm.Input;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.States
{
    internal class PlayerStateInteract : PlayerStateBase
    {
        private List<BaseTile> _playerNeighbors;
        private BaseTile? _selectedTile;
        private Direction _selectedDirection;
        private DisplayManager _displayManager;

        public PlayerStateInteract(Player player, DisplayManager displayManager) : base(player)
        {
            _playerNeighbors = new List<BaseTile>();
            _displayManager = displayManager;
        }
        public override void Enter()
        {
            ExamineSameTile();
            _playerNeighbors = player.mapManager.GetNeighbours(player.GridPosition);
        }

        public override void Exit()
        {
            _playerNeighbors.Clear();
            _selectedTile = null;
            _displayManager.ClearMessageSection();
        }

        public override void MoveInput(Vector2Int direction)
        {
            ExamineNeighbor(direction);
        }


        public override void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    if (_selectedTile != null)
                    {
                        player.InteractWithNeighbor(_selectedTile);
                        player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
                    }
                    break;
                case InputCommands.Cancel:
                    player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
                    break;
                case InputCommands.Interact:
                    ExamineSameTile();
                    break;
                case InputCommands.Inventory:
                    break;
                case InputCommands.Next:
                    break;
                case InputCommands.Previous:
                    break;
                case InputCommands.Help:
                    break;
                default:
                    break;
            }
        }
        private void ExamineSameTile()
        {
            player.mapManager.TileGrid.TryGetGridObject(player.GridPosition, out _selectedTile);
            PrintSelectedTile($"You are standing on {_selectedTile.Label}");
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

            _selectedTile = _playerNeighbors.Find(n => n.GridPosition.Equals(player.GridPosition.Add(direction)));
            if (_selectedTile != null)
            {
                PrintSelectedTile($"Looking {_selectedDirection.ToString().ToLower()} at {_selectedTile.Label}");
            }
        }

        private void PrintSelectedTile(string baseText)
        {
            if (_selectedTile is IOccupiable occupiable && occupiable.ObjectsOnTile.Count > 0)
            {
                if (occupiable.ObjectsOnTile.Count == 1 && occupiable.ObjectsOnTile.Peek() is Player)
                    baseText += ". You are alone here.";
                else
                {
                    baseText += ". Objects on tile:";
                    foreach (var obj in occupiable.ObjectsOnTile)
                    {
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
