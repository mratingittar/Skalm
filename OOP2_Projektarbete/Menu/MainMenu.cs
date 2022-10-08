namespace Skalm.Menu
{
    internal class MainMenu : MenuBase
    {
        public MainMenu(string[] title, TreeNode<MenuPage> pages) : base(title, pages)
        {
            base.pages.AddChildren(
                new MenuPage("New Game"),
                new MenuPage("Continue"),
                new MenuPage("Options", "Select Input Method", "Select Music", "Toggle Beep", "Back"));

            base.pages.Children.First(page => page.Value.pageName == "Options").AddChildren(
                new MenuPage("Select Input Method"), 
                new MenuPage("Select Music"));
        }
    }
}
