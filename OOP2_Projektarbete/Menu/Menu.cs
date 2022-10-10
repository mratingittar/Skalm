using Skalm.Display;

namespace Skalm.Menu
{
    internal class Menu
    {
        #region FIELDS
        public readonly string[] title;
        public readonly TreeNode<MenuPage> pages;
        private int pageStartRow;
        private DisplayManager displayManager;
        public event Action<string, string>? onMenuExecution;
        #endregion

        #region CONSTRUCTOR
        public Menu(string[] title, TreeNode<MenuPage> pages, DisplayManager displayManager)
        {
            this.title = title;
            this.pages = pages;
            this.displayManager = displayManager;
            IsEnabled = false;
            ActivePage = pages.First().Value;
        }
        #endregion

        #region PROPERTIES
        public bool IsEnabled { get; set; }
        public MenuPage ActivePage { get; private set; }
        public int MenuLevel => pages.FindNode(node => node.Value == ActivePage).Depth;
        public int MenuItemIndex { get; set; }
        #endregion

        public void LoadMenu(int titlePadding)
        {
            IsEnabled = true;
            MenuItemIndex = 0;
            displayManager.printer.PrintCenteredInWindow(title, titlePadding);
            pageStartRow = displayManager.windowInfo.CursorPosition.Item2 + titlePadding;
            LoadPage(pages.Value);
        }
        public void LoadPage(MenuPage page)
        {
            displayManager.eraser.EraseLinesFromTo(pageStartRow, pageStartRow + ActivePage.items.Count + 2); // MAGIC NUMBER; INTRODUCE FIELD OR CONSTANT
            ActivePage = page;
            MenuItemIndex = 0;
            PrintMenu();
        }

        public void ExecuteSelectedMenuItem()
        {
            string item = ActivePage.items[MenuItemIndex];
            if (item == "Back")
                GoBackOneLevel();
            else if (item == "Exit")
                Environment.Exit(0);
            else
            {
                if (pages.FindNode(node => node.Value.pageName.ToUpper() == item.ToUpper()) is null)
                    ExecuteLeaf(item);
                else
                    LoadPage(pages.FindNode(node => node.Value.pageName.ToUpper() == item.ToUpper()).Value);
            }
        }

        private void ExecuteLeaf(string executedItem)
        {
            onMenuExecution?.Invoke(ActivePage.pageName, executedItem);
        }

        public bool MoveMenuUp()
        {
            if (MenuItemIndex == 0)
                return false;

            MenuItemIndex--;
            PrintMenu();
            return true;
        }
        public bool MoveMenuDown()
        {
            if (MenuItemIndex == ActivePage.items.Count - 1)
                return false;

            MenuItemIndex++;
            PrintMenu();
            return true;
        }

        public void GoBackOneLevel()
        {
            LoadPage(pages.FindNode(node => node.Value.pageName.ToUpper() == ActivePage.pageName.ToUpper()).Parent!.Value);
        }

        private void PrintMenu()
        {
            displayManager.printer.PrintCenteredInWindow(ActivePage.pageName, pageStartRow);
            displayManager.printer.PrintCenteredInWindow("", pageStartRow + 1); // MAGIC NUMBER; INTRODUCE FIELD OR CONSTANT
            HighlightSelectedItem(pageStartRow + 2); // MAGIC NUMBER; INTRODUCE FIELD OR CONSTANT
        }

        private void HighlightSelectedItem(int startRow)
        {
            int count = 0;
            foreach (var item in ActivePage.items)
            {
                if (item.Key == MenuItemIndex)
                    displayManager.printer.PrintCenteredInWindow(item.Value, startRow + count, true);
                else
                    displayManager.printer.PrintCenteredInWindow(item.Value, startRow + count);
                count++;
            }
        }


    }
}
