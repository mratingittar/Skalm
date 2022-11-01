

namespace Skalm.Menu
{
    internal class MenuPage
    {
        public readonly Page page;
        public readonly string pageName;
        public readonly Dictionary<int, string> items = new();

        public MenuPage(Page page, string name, params string[] items)
        {
            this.page = page;
            pageName = name;
            for (int i = 0; i < items.Length; i++)
            {
                this.items.Add(i, items[i]);
            }
        }
    }
    public enum Page
    {
        MainMenu,
        PauseMenu,
        NewGame,
        Options,
        InputMethod,
        Music,
        Credits,
        HowToPlay
    }
}
