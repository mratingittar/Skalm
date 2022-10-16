namespace Skalm.Utilities
{
    internal class DefaultSettings : ISettings
    {
        #region PROPERTIES
        public string GameTitle { get; private set; } = "Skälm";
        public bool DisplayCursor { get; private set; } = false;
        public int UpdateFrequency { get; private set; } = 20;

        public int WindowPadding { get; private set; } = 1;
        public int BorderThickness { get; private set; } = 1;
        public int CellWidth { get; private set; } = 2;
        public int CellHeight { get; private set; } = 1;
        public int MapWidth { get; private set; } = 42;
        public int MapHeight { get; private set; } = 32;
        public int MessageBoxHeight { get; private set; } = 5; // MINIMUM 3!
        public int StatsWidth { get; private set; } = 23; // MINIMUM 23!
        public int MainStatsHeight { get; private set; } = 8; // MINIMUM 8!
        public int HudPadding { get; private set; } = 1;

        public char SpriteBorder { get; private set; } = '░';
        public char SpriteWall { get; private set; } = '#';
        public char SpriteFloor { get; private set; } = '∙';
        public char SpriteDoorClosed { get; private set; } = '■';
        public char SpriteDoorOpen { get; private set; } = '□';

        public ConsoleColor ForegroundColor { get; private set; } = ConsoleColor.White;

        public ConsoleColor BackgroundColor { get; private set; } = ConsoleColor.Black;
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
