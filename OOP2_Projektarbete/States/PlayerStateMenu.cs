using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Items;
using Skalm.Input;
using Skalm.Structs;

namespace Skalm.States
{
    internal class PlayerStateMenu : PlayerStateBase
    {
        private DisplayManager _displayManager;
        public PlayerStateMenu(Player player, DisplayManager displayManager) : base(player)
        {
            _displayManager = displayManager;
        }

        public override void Enter()
        {
            _displayManager.pixelGridController.InventoryIndex = 0;
            player.UpdateInventoryDisplay();
            DescriptionAsMessage();
        }

        public override void Exit()
        {
            _displayManager.pixelGridController.InventoryIndex = -1;
            player.UpdateInventoryDisplay();
            _displayManager.ClearMessageSection();
        }
        public override void MoveInput(Vector2Int direction)
        {
            switch (direction.Y)
            {
                case < 0:
                    if (_displayManager.pixelGridController.InventoryIndex > 0)
                        _displayManager.pixelGridController.InventoryIndex--;
                    break;
                case > 0:
                    if (_displayManager.pixelGridController.InventoryIndex < player.equipmentManager.inventory.itemList.Count() - 1)
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
            player.UpdateInventoryDisplay();
            DescriptionAsMessage();
        }

        private void DescriptionAsMessage()
        {
            if (player.equipmentManager.inventory.itemList.Count > 0)
                _displayManager.DisplayInstantMessage(player.equipmentManager.inventory.itemList[_displayManager.pixelGridController.InventoryIndex].Name + ". " +
                    player.equipmentManager.inventory.itemList[_displayManager.pixelGridController.InventoryIndex].Description + ".");
            else
                _displayManager.DisplayInstantMessage("Your inventory is empty.");
        }

        public override void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    UseSelectedItem(player.equipmentManager.inventory.itemList[_displayManager.pixelGridController.InventoryIndex]);
                    break;
                case InputCommands.Cancel:
                    player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
                    break;
                case InputCommands.Interact:
                    player.equipmentManager.inventory.RemoveItem(player.equipmentManager.inventory.itemList[_displayManager.pixelGridController.InventoryIndex]);
                    player.UpdateInventoryDisplay();
                    break;
                case InputCommands.Inventory:
                    player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
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

        private void JumpMenu(bool forwards)
        {
            if (forwards)
                _displayManager.pixelGridController.InventoryIndex = 
                    Math.Min(_displayManager.pixelGridController.InventoryIndex + _displayManager.pixelGridController.InventoryRowsAvailable, 
                    player.equipmentManager.inventory.itemList.Count() - 1);
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
                item.Use(player);
                player.UpdateStatDisplay();
                player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMove);
            }
        }
    }
}
