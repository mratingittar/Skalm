using Skalm.GameObjects;
using Skalm.GameObjects.Interfaces;
using Skalm.Input;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.States
{
    internal class PlayerStateLook : PlayerStateBase
    {
        private List<BaseTile> _playerNeighbors;
        private BaseTile? _selectedNeigbor;
        private Direction _selectedDirection;

        public static event Action<string>? OnNeighborSelected;

        public PlayerStateLook(Player player) : base(player) 
        {
            _playerNeighbors = new List<BaseTile>();
        }
        public override void Enter()
        {
            ExamineSameTile();
            _playerNeighbors = player.mapManager.GetNeighbours(player.GridPosition);
        }

        public override void Exit()
        {
            _playerNeighbors.Clear();
            _selectedNeigbor = null;
            OnNeighborSelected?.Invoke("");
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
                    if (_selectedNeigbor != null)
                    {
                        player.InteractWithNeighbor(_selectedNeigbor);
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
            player.mapManager.TileGrid.TryGetGridObject(player.GridPosition, out _selectedNeigbor);
            PrintSelectedTile($"You are standing on {_selectedNeigbor.Label}");
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

            _selectedNeigbor = _playerNeighbors.Find(n => n.GridPosition.Equals(player.GridPosition.Add(direction)));
            if (_selectedNeigbor != null)
            {
                PrintSelectedTile($"Looking {_selectedDirection.ToString().ToLower()} at {_selectedNeigbor.Label}.");
            }
        }

        private void PrintSelectedTile(string baseText)
        {
            if (_selectedNeigbor is IOccupiable occupiable && occupiable.ObjectsOnTile.Count > 0)
            {
                baseText += " Objects on tile:";
                foreach (var obj in occupiable.ObjectsOnTile)
                {
                    baseText += $" {obj.Label},";
                }
                baseText = baseText.Remove(baseText.Length - 1);
                baseText += ".";
            }

            OnNeighborSelected?.Invoke(baseText);
        }
    }
}
