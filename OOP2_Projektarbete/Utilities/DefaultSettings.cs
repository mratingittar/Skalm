namespace Skalm.Utilities
{
    internal class DefaultSettings : ISettings
    {
        #region PROPERTIES
        public string GameTitle { get; } = "Skälm";
        public int UpdateFrequency { get; } = 20;
        public float BaseSpawningModifier { get; } = 0.65f;

        public int WindowPadding { get; } = 1;
        public int BorderThickness { get; } = 1;
        public int CellWidth { get; } = 2;
        public int CellHeight { get; } = 1;
        public int MapWidth { get; } = 42;
        public int MapHeight { get; } = 42;
        public int MessageBoxHeight { get; } = 5; // MINIMUM 3!
        public int StatsWidth { get; } = 23; // MINIMUM 23!
        public int MainStatsHeight { get; } = 11; // MINIMUM 11!
        public int HudPadding { get; } = 1;

        public char BorderSprite { get; } = '░';
        public char WallSprite { get; } = '#';
        public char FloorSprite { get; } = '∙';
        public char DoorSpriteOpen { get; } = '□';
        public char DoorSpriteClosed { get; } = '■';

        public char GoalSprite { get; } = '▼';
        public char PlayerSprite { get; } = '℗';
        public char EnemySprite { get; } = 'ⱺ';
        public char KeySprite { get; } = 'Ⱡ';
        public char PotionSprite { get; } = '♥';
        public char ItemSprite { get; } = '$';

        public ConsoleColor GoalColor { get; } = ConsoleColor.Green;
        public ConsoleColor PlayerColor { get; } = ConsoleColor.Blue;
        public ConsoleColor EnemyColor { get; } = ConsoleColor.Magenta;
        public ConsoleColor ItemColor { get; } = ConsoleColor.Yellow;
        public ConsoleColor PotionColor { get; } = ConsoleColor.Red;
        public ConsoleColor KeyColor { get; } = ConsoleColor.White;
        public ConsoleColor HUDColor { get; } = ConsoleColor.Gray;
        public ConsoleColor TextColor { get; } = ConsoleColor.White;
        public ConsoleColor BackgroundColor { get; } = ConsoleColor.Black;

        #endregion

        public bool LoadSettings(string[] file)
        {
            return FileHandler.WriteFile("settings.txt", CreateSettingsFile());
        }

        private string[] CreateSettingsFile()
        {
            List<string> settingsList = new List<string>
            {
                "# Settings file for Skälm",
                "# Edit at your own risk!",
                "",
                "# This is a comment.",
                "# Write properties like this:",
                "# Type Property = Value",
                ""
            };

            foreach (var item in GetType().GetProperties())
            {
                settingsList.Add(item.PropertyType.Name.ToString() + " " + item.Name + " = " + item.GetValue(this)!.ToString());
            }

            return settingsList.ToArray();
        }
    }
}
