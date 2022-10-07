using Skalm.Display;
using Skalm.Input;
using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.Menu
{
    internal class MenuManager
    {
        private InputManager inputManager;
        private readonly TreeNode<IMenu> mainMenuTree;
        private TreeNode<IMenu> currentNode;
        private int selectedMenuItemIndex;
        private Bounds menuBounds;
        private int menuItemsRowStart;
        private AsciiArt ascii;
        public MenuManager(InputManager inputManager)
        {
            this.inputManager = inputManager;
            mainMenuTree = new TreeNode<IMenu>(new MainMenuRoot("Main Menu", new MenuItems("New Game", "Continue", "Options", "Exit")));
            mainMenuTree.AddChild(new OptionsMenu("Options", new MenuItems("Select Input Method", "Select Music", "Toggle Beep")));
            currentNode = mainMenuTree;

            inputManager.onInputMove += TraverseMenu;
            inputManager.onInputCommand += ExecuteMenu;
            ascii = new AsciiArt();
        }

        public void LoadMainMenu()
        {
            DisplayManager.PrintCenteredMultiLineText(ascii.SkalmTitle, 5);
            menuItemsRowStart = Console.CursorTop += 5;
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
                        LoadMenu(currentNode.Children.First(n => n.Value.MenuName == currentNode.Value.Items.Values[selectedMenuItemIndex]).Value, true);
                    break;
                case InputCommands.Cancel:
                    if (currentNode.Parent is not null)
                        LoadMenu(currentNode.Parent.Value, true);
                    else
                        Environment.Exit(0);
                    break;

            }
        }


        private void RunCommand()
        {
            throw new NotImplementedException();
        }

        private void UnloadMenu()
        {

        }

        private void LoadMenu(IMenu menu, bool unloadOldMenu = false)
        {
            if (unloadOldMenu)
                DisplayManager.Tippex(menuBounds);

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
            string[] allLines = currentNode.Value.Items.Values.Values.ToArray();
            int maxWidth = allLines.Max(x => x.Length);
            menuBounds = new Bounds(new Vector2Int(Console.WindowWidth / 2 - maxWidth / 2, menuItemsRowStart),
                new Rectangle(maxWidth, allLines.Length));

            for (int i = 0; i < allLines.Length; i++)
            {
                DisplayManager.Print(currentNode.Value.Items.Values[i], 0, menuItemsRowStart + i, true, i == selectedMenuItemIndex);
            }
        }
    }
}
