using Skalm.Display;
using Skalm.Input;
using Skalm.Sounds;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Menu
{
    internal class MenuManager
    {
        private InputManager inputManager;
        private readonly TreeNode<IMenu> mainMenuTree;
        private TreeNode<IMenu> currentNode;
        private int selectedMenuItemIndex;
        private int menuItemsRowStart;

        public MenuManager(InputManager inputManager)
        {
            this.inputManager = inputManager;
            mainMenuTree = new TreeNode<IMenu>(new MainMenuRoot("Main Menu", new MenuItems("New Game", "Continue", "Options", "Exit")));
            mainMenuTree.AddChild(new OptionsMenu("Options", new MenuItems("Select input method", "Select music", "Toggle beep")));
            currentNode = mainMenuTree;

            inputManager.onInputMove += TraverseMenu;
            inputManager.onInputCommand += ExecuteMenu;

        }

        public void LoadMainMenu()
        {
            LoadMenu(currentNode.Value);
        }
        private void TraverseMenu(Vector2Int direction)
        {
            if (!currentNode.Value.Enabled)
                return;

            switch (direction.Y)
            {
                case < 0:
                    MoveMenuDown();
                    break;
                case > 0:
                    MoveMenuUp();
                    break;
            }
        }
        private void ExecuteMenu(InputCommands command)
        {
            if (currentNode.Value.Enabled is false)
                return;

            SoundManager.PlayConfirmBeep();
            switch (command)
            {
                case InputCommands.Confirm:
                    if (currentNode.Children.Count == 0)
                        RunCommand();
                    else
                        LoadMenu(currentNode.Children.First(n => n.Value.MenuName == currentNode.Value.Items.Values[selectedMenuItemIndex]).Value);
                    break;
                case InputCommands.Cancel:
                    if (currentNode.Parent is not null)
                        LoadMenu(currentNode.Parent.Value);
                    break;

            }
        }


        private void RunCommand()
        {
            throw new NotImplementedException();
        }

        private void LoadMenu(IMenu menu)
        {
            selectedMenuItemIndex = 0;
            currentNode = mainMenuTree.ReturnNodeWithChildren().First(n => n.Value == menu);
            currentNode.Value.Enabled = true;
            PrintMenuChoices();
        }

        private void MoveMenuUp()
        {
            if (selectedMenuItemIndex == 0)
                return;

            selectedMenuItemIndex--; 
            SoundManager.PlayMoveBeep();
            PrintMenuChoices();
        }

        private void MoveMenuDown()
        {
            if (selectedMenuItemIndex == currentNode.Value.Items.Values.Count - 1)
                return;

            selectedMenuItemIndex++;
            SoundManager.PlayMoveBeep();
            PrintMenuChoices();
        }

        private void PrintMenuChoices()
        {
            Console.CursorTop = menuItemsRowStart;
            Console.WriteLine();
            foreach (KeyValuePair<int,string> item in currentNode.Value.Items.Values)
            {
                if (item.Key == selectedMenuItemIndex)
                    PrintWithHighlight(item.Value);
                else
                    DisplayManager.PrintCenteredText(item.Value, Console.GetCursorPosition().Top);
            }
        }

        private void PrintWithHighlight(string text)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            DisplayManager.PrintCenteredText(text, Console.GetCursorPosition().Top);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
