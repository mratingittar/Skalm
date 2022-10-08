using Skalm.Display;
using Skalm.Sounds;
using Skalm.Structs;

namespace Skalm.Menu
{
    internal abstract class MenuBase
    {
        #region FIELDS
        public readonly string[] title;
        public readonly TreeNode<MenuPage> pages;
        private int pageStartRow;
        public int MenuItemIndex { get; set; }
        #endregion

        #region CONSTRUCTOR
        protected MenuBase(string[] title, TreeNode<MenuPage> pages)
        {
            this.title = title;
            this.pages = pages;
            IsEnabled = false;
            ActivePage = pages.First().Value;
        } 
        #endregion

        #region PROPERTIES
        public bool IsEnabled { get; set; }
        public MenuPage ActivePage { get; private set; }
        public int MenuLevel => pages.FindNode(node => node.Value == ActivePage).Depth; 
        #endregion

        public void LoadMenu(int titlePadding)
        {
            IsEnabled = true;
            MenuItemIndex = 0;
            DisplayManager.PrintCentered(title, titlePadding);
            pageStartRow = Console.CursorTop + titlePadding;
            LoadPage(pages.Value, false);
        }
        public void LoadPage(MenuPage page, bool skipErase = true)
        { 
            // MISSING: DELETING OLD PAGE WITHOUT DELETING TITLE
            ActivePage = page;
            if (skipErase)
                DisplayManager.EraseLatestPrint();
            HighlightSelectedItem();
        }      
        
        public void ExecuteSelectedMenuItem()
        {
            string item = ActivePage.items[MenuItemIndex];
            if (item == "Back")
                GoBackOneLevel();
            else
            {
            MenuPage page = pages.FindNode(node => node.Value.pageName.ToUpper() == item.ToUpper()).Value;
            LoadPage(page);
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
            if (MenuItemIndex == ActivePage.items.Count-1)
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
                    DisplayManager.PrintCentered(item.Value, pageStartRow + count, true);
                else
                    DisplayManager.PrintCentered(item.Value, pageStartRow + count);
                count++;
            }
        }


    }
}
