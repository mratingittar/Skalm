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
            string selection = $"You are standing on {_selectedNeigbor.Label}.";
            OnNeighborSelected?.Invoke(selection);
        }

        private void ExamineNeighbor(Vector2Int direction)
        {
            if (direction.Equals(Vector2Int.Up))
                _selectedDirection = Direction.Up;
            else if (direction.Equals(Vector2Int.Right))
                _selectedDirection = Direction.Right;
            else if (direction.Equals(Vector2Int.Down))
                _selectedDirection = Direction.Down;
            else if (direction.Equals(Vector2Int.Left))
                _selectedDirection = Direction.Left;

            _selectedNeigbor = _playerNeighbors.Find(n => n.GridPosition.Equals(player.GridPosition.Add(direction)));
            if (_selectedNeigbor != null)
            {
                string selectionText = $"Looking {_selectedDirection.ToString().ToLower()} at {_selectedNeigbor.Label}.";

                if (_selectedNeigbor is IOccupiable occupiable && occupiable.ObjectsOnTile.Count > 0)
                {
                    selectionText += " Objects on tile:";
                    foreach (var obj in occupiable.ObjectsOnTile)
                    {
                        selectionText += $" {obj.Label} ";
                    }
                }

                OnNeighborSelected?.Invoke(selectionText);
            }
        }
    }
}
