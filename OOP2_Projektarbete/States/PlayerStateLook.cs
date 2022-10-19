using Skalm.GameObjects;
using Skalm.GameObjects.Interfaces;
using Skalm.Input;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class PlayerStateLook : PlayerStateBase
    {
        private List<BaseTile> _playerNeighbors;
        private BaseTile? _selectedNeigbor;
        private Direction selectedDirection;

        public static event Action<string>? onNeighborSelected;

        public PlayerStateLook(Player player) : base(player) 
        {
            _playerNeighbors = new List<BaseTile>();
        }
        public override void Enter()
        {
            onNeighborSelected?.Invoke("Choose a direction to interact in.");
            _playerNeighbors = player.mapManager.GetNeighbours(player.GridPosition);
        }

        public override void Exit()
        {
            _playerNeighbors.Clear();
            _selectedNeigbor = null;
            onNeighborSelected?.Invoke("");
        }

        public override void MoveInput(Vector2Int direction)
        {
            SelectNeighborForInteraction(direction);
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
        private void SelectNeighborForInteraction(Vector2Int direction)
        {

            if (direction.Equals(Vector2Int.Up))
                selectedDirection = Direction.Up;
            else if (direction.Equals(Vector2Int.Right))
                selectedDirection = Direction.Right;
            else if (direction.Equals(Vector2Int.Down))
                selectedDirection = Direction.Down;
            else if (direction.Equals(Vector2Int.Left))
                selectedDirection = Direction.Left;

            _selectedNeigbor = _playerNeighbors.Find(n => n.GridPosition.Equals(player.GridPosition.Add(direction)));
            if (_selectedNeigbor != null)
            {
                string selectionText = $"Selected {selectedDirection}: {_selectedNeigbor.GetType().Name}.";

                if (_selectedNeigbor is IOccupiable occupiable && occupiable.ObjectsOnTile.Count > 0)
                {
                    selectionText += " Objects on tile:";
                    foreach (var obj in occupiable.ObjectsOnTile)
                    {
                        selectionText += $" {obj.GetType().Name} ";
                    }
                }

                onNeighborSelected?.Invoke(selectionText);
            }
        }
    }
}
