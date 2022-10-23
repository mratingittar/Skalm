using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Items;
using Skalm.Input;
using Skalm.Structs;

namespace Skalm.States
{
    internal class PlayerStateMenu : IPlayerState
    {
        private Player _player;
        private DisplayManager _displayManager;

        public PlayerStateMenu(Player player, DisplayManager displayManager)
        {
            _player = player;
            _displayManager = displayManager;
        }

        public void Enter()
        {
            _displayManager.pixelGridController.InventoryIndex = 0;
            _player.UpdateInventoryDisplay();
            DescriptionAsMessage();
        }

        public void Exit()
        {
            _displayManager.pixelGridController.InventoryIndex = -1;
            _player.UpdateInventoryDisplay();
            _displayManager.ClearMessageSection();
        }
        public void MoveInput(Vector2Int direction)
        {
            switch (direction.Y)
            {
                case < 0:
                    if (_displayManager.pixelGridController.InventoryIndex > 0)
                        _displayManager.pixelGridController.InventoryIndex--;
                    break;
                case > 0:
                    if (_displayManager.pixelGridController.InventoryIndex < _player.equipmentManager.inventory.itemList.Count() - 1)
                        _displayManager.pixelGridController.InventoryIndex++;
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
            if (_player.equipmentManager.inventory.itemList.Count > 0)
                _displayManager.DisplayInstantMessage(_player.equipmentManager.inventory.itemList[_displayManager.pixelGridController.InventoryIndex].Name + ". " +
                    _player.equipmentManager.inventory.itemList[_displayManager.pixelGridController.InventoryIndex].Description + ".");
            else
                _displayManager.DisplayInstantMessage("Your inventory is empty.");
        }

        public void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    UseSelectedItem(_player.equipmentManager.inventory.itemList[_displayManager.pixelGridController.InventoryIndex]);
                    break;
                case InputCommands.Cancel:
                    _player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
                    break;
                case InputCommands.Interact:
                    _player.equipmentManager.inventory.RemoveItem(_player.equipmentManager.inventory.itemList[_displayManager.pixelGridController.InventoryIndex]);
                    _player.UpdateInventoryDisplay();
                    break;
                case InputCommands.Inventory:
                    _player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
                    break;
            }
        }

        private void JumpMenu(bool forwards)
        {
            if (forwards)
                _displayManager.pixelGridController.InventoryIndex = 
                    Math.Min(_displayManager.pixelGridController.InventoryIndex + _displayManager.pixelGridController.InventoryRowsAvailable, 
                    _player.equipmentManager.inventory.itemList.Count() - 1);
            else
                _displayManager.pixelGridController.InventoryIndex = 
                    Math.Max(0, 
                    _displayManager.pixelGridController.InventoryIndex - _displayManager.pixelGridController.InventoryRowsAvailable);
        }

        private void UseSelectedItem(Item item)
        {
            if (item is Key)
                _displayManager.DisplayInstantMessage("Interact with a locked door to use.");
            else
            {
                item.Use(_player);
                _player.UpdateStatDisplay();
                _player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
            }
        }
    }
}
