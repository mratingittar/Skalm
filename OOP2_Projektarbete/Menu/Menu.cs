using Skalm.Display;
using Skalm.Sounds;

namespace Skalm.Menu
{
    internal class Menu
    {
        #region FIELDS
        public readonly string[] title;
        public readonly TreeNode<MenuPage> pages;
        private int pageStartRow;
        private DisplayManager displayManager;
        #endregion

        #region CONSTRUCTOR
        public Menu(string[] title, TreeNode<MenuPage> pages, DisplayManager displayManager)
        {
            this.title = title;
            this.pages = pages;
            this.displayManager = displayManager;
            IsEnabled = false;
            ActivePage = pages.First().Value;

            pages.AddChildren(
               new MenuPage("New Game"),
               new MenuPage("Continue"),
               new MenuPage("Options", "Select Input Method", "Select Music", "Toggle Beep", "Back"));

            pages.Children.First(page => page.Value.pageName == "Options").AddChildren(
                new MenuPage("Select Input Method"),
                new MenuPage("Select Music"));
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
            pageStartRow = displayManager.CurrentCursorPosition.Item2 + titlePadding;
            LoadPage(pages.Value);
        }
        public void LoadPage(MenuPage page)
        {
            ActivePage = page;
            displayManager.eraser.EraseLinesFromTo(pageStartRow, pageStartRow + page.items.Count);
            HighlightSelectedItem();
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
                LoadPage(pages.FindNode(node => node.Value.pageName.ToUpper() == item.ToUpper()).Value);
            }
        }

        public void MoveMenuUp()
        {
            if (MenuItemIndex == 0)
                return;

            MenuItemIndex--;
            SoundManager.PlayMoveBeep();
            HighlightSelectedItem();
        }
        public void MoveMenuDown()
        {
            if (MenuItemIndex == ActivePage.items.Count - 1)
                return;

            MenuItemIndex++;
            SoundManager.PlayMoveBeep();
            HighlightSelectedItem();
        }

        public void GoBackOneLevel()
        {
            LoadPage(pages.FindNode(node => node.Value.pageName.ToUpper() == ActivePage.pageName.ToUpper()).Parent!.Value);
        }

        private void HighlightSelectedItem()
        {
            int count = 0;
            foreach (var item in ActivePage.items)
            {
                if (item.Key == MenuItemIndex)
                    displayManager.printer.PrintCenteredInWindow(item.Value, pageStartRow + count, true);
                else
                    displayManager.printer.PrintCenteredInWindow(item.Value, pageStartRow + count);
                count++;
            }
        }


    }
}
