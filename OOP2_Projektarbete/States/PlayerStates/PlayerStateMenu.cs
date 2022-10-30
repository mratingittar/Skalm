using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Items;
using Skalm.Input;
using Skalm.Structs;

namespace Skalm.States.PlayerStates
{
    internal class PlayerStateMenu : IPlayerState
    {
        private Player _player;
        private DisplayManager _displayManager;
        private List<Item> _items;

        public PlayerStateMenu(Player player, DisplayManager displayManager)
        {
            _player = player;
            _displayManager = displayManager;
            _items = new List<Item>();
        }

        public void Enter()
        {
            _items = _player.EquipmentManager.inventory.itemList.Where(i => i is not Key).ToList();
            _displayManager.PixelGridController.InventoryIndex = 0;
            _player.UpdateInventoryDisplay();
            DescriptionAsMessage();
        }

        public void Exit()
        {
            _displayManager.PixelGridController.InventoryIndex = -1;
            _player.UpdateInventoryDisplay();
            _displayManager.ClearMessageSection();
        }
        public void MoveInput(Vector2Int direction)
        {
            switch (direction.Y)
            {
                case < 0:
                    if (_displayManager.PixelGridController.InventoryIndex > 0)
                        _displayManager.PixelGridController.InventoryIndex--;
                    break;
                case > 0:
                    if (_displayManager.PixelGridController.InventoryIndex < _items.Count() - 1)
                        _displayManager.PixelGridController.InventoryIndex++;
                    break;
            }
            switch (direction.X)
            {
                case < 0:
                    JumpMenu(false);
                    break;
                case > 0:
                    JumpMenu(true);
                    break;
            }
            _player.UpdateInventoryDisplay();
            DescriptionAsMessage();
        }

        private void DescriptionAsMessage()
        {
            if (_items.Count > 0 && _items.Count > _displayManager.PixelGridController.InventoryIndex)
                _displayManager.DisplayInstantMessage(_items[_displayManager.PixelGridController.InventoryIndex].Name + ". " +
                    _items[_displayManager.PixelGridController.InventoryIndex].Description + ".");
            else
                _displayManager.DisplayInstantMessage("Nothing to see here.");
        }

        public void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    if (_items.Count() > 0)
                        UseSelectedItem(_items[_displayManager.PixelGridController.InventoryIndex]);
                    break;
                case InputCommands.Cancel:
                    _player.PlayerStateMachine.ChangeState(EPlayerStates.PlayerStateMove);
                    break;
                case InputCommands.Interact:
                    if (_items.Count() > 0)
                    {
                        _player.EquipmentManager.inventory.RemoveItem(_items[_displayManager.PixelGridController.InventoryIndex]);
                        _displayManager.PixelGridController.InventoryIndex = 0;
                        _player.UpdateInventoryDisplay();
                    }
                    break;
                case InputCommands.Inventory:
                    _player.PlayerStateMachine.ChangeState(EPlayerStates.PlayerStateMove);
                    break;
            }
        }

        private void JumpMenu(bool forwards)
        {
            if (forwards)
                _displayManager.PixelGridController.InventoryIndex =
                    Math.Min(_displayManager.PixelGridController.InventoryIndex + _displayManager.PixelGridController.InventoryRowsAvailable,
                    _items.Count() - 1);
            else
                _displayManager.PixelGridController.InventoryIndex =
                    Math.Max(0,
                    _displayManager.PixelGridController.InventoryIndex - _displayManager.PixelGridController.InventoryRowsAvailable);
        }

        private void UseSelectedItem(Item item)
        {
            if (item is Key)
                _displayManager.DisplayInstantMessage("Interact with a locked door to use.");
            else
            {
                item.Use(_player);
                _player.UpdateAllDisplays();
                _player.PlayerStateMachine.ChangeState(EPlayerStates.PlayerStateMove);
            }
        }
    }
}
