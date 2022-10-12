namespace Skalm
{
    internal class DefaultSettings : Settings
    {
        public new string GameTitle { get; private set; } = "Skälm";
        public new bool DisplayCursor { get; private set; } = false;
        public new int UpdateFrequency { get; private set; } = 20;

        public new int WindowPadding { get; private set; } = 1;
        public new int BorderThickness { get; private set; } = 1;
        public new int CellWidth { get; private set; } = 2;
        public new int CellHeight { get; private set; } = 1;
        public new int MapWidth { get; private set; } = 42;
        public new int MapHeight { get; private set; } = 32;
        public new int MessageBoxHeight { get; private set; } = 5;
        public new int StatsWidth { get; private set; } = 24;
        public new int MainStatsHeight { get; private set; } = 8;
        public new int SubStatsHeight { get; private set; } = 28;
        public new int HudPadding { get; private set; } = 1;

        public new char SpriteBorder { get; private set; } = '░';
        public new char SpriteWall { get; private set; } = '#';
        public new char SpriteFloor { get; private set; } = '.';
        public new char SpriteDoor { get; private set; } = '+';
        
        public new string SoundsFolderPath { get; private set; } = "..\\..\\..\\Sounds\\";

        public override bool LoadSettings(string[] file)
        {
            FileHandler.WriteFile("settings.txt", CreateSettingsFile());
            return true;
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
                "# type property = value",
                ""
            };

            foreach (var item in this.GetType().GetProperties())
            {
                settingsList.Add(item.PropertyType.Name.ToString() + " " + item.Name + " = " + item.GetValue(this)!.ToString());
            }
            
            return settingsList.ToArray();
        }
    }
}
