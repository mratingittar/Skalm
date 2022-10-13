namespace Skalm.Utilities
{
    internal class Settings : ISettings
    {
        public string GameTitle { get; private set; } = "";
        public bool DisplayCursor { get; private set; }
        public int UpdateFrequency { get; private set; }

        public int WindowPadding { get; private set; }
        public int BorderThickness { get; private set; }
        public int CellWidth { get; private set; }
        public int CellHeight { get; private set; }
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        public int MessageBoxHeight { get; private set; }
        public int StatsWidth { get; private set; }
        public int MainStatsHeight { get; private set; }
        public int HudPadding { get; private set; }

        public char SpriteBorder { get; private set; }
        public char SpriteWall { get; private set; }
        public char SpriteFloor { get; private set; }
        public char SpriteDoor { get; private set; }

        public string SoundsFolderPath { get; private set; } = "";

        public ConsoleColor ForegroundColor { get; private set; }

        public ConsoleColor BackgroundColor { get; private set; }

        public virtual bool LoadSettings(string[] settingsFile)
        {
            bool success = ApplySettings(settingsFile);
            CheckForMinValues();
            return success;
            // ADD REFERENCE TO DEFAULTSETTINGS, LOAD SINGLE MISSING SETTINGS FROM THAT
        }

        private void CheckForMinValues()
        {
            StatsWidth = Math.Max(StatsWidth, 23);
            MainStatsHeight = Math.Max(MainStatsHeight, 8);
            MessageBoxHeight = Math.Max(MessageBoxHeight, 3);
        }

        private bool ApplySettings(string[] settingsFile)
        {
            foreach (var line in settingsFile)
            {
                if (line.Trim() == "")
                    continue;

                string type = line.Split(' ')[0];
                if (type == "#")
                    continue;

                string name = line.Split(' ')[1];
                string value = line.Split('=').Last().Trim();

                if (GetType().GetProperty(name) is null)
                    return false;

                try
                {
                    switch (type)
                    {
                        case "Int32":
                            GetType().GetProperty(name)!.SetValue(this, ParseInt(value));
                            break;
                        case "Char":
                            GetType().GetProperty(name)!.SetValue(this, ParseChar(value));
                            break;
                        case "String":
                            GetType().GetProperty(name)!.SetValue(this, value);
                            break;
                        case "Boolean":
                            GetType().GetProperty(name)!.SetValue(this, ParseBool(value));
                            break;
                        case "ConsoleColor":
                            GetType().GetProperty(name)!.SetValue(this, ParseColor(value));
                            break;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }



        private char ParseChar(string field)
        {
            return field.ToCharArray().SingleOrDefault();
        }

        private int ParseInt(string field)
        {
            int.TryParse(field, out int value);
            return value;
        }

        private bool ParseBool(string field)
        {
            bool.TryParse(field, out bool value);
            return value;
        }

        private ConsoleColor ParseColor(string field)
        {
            ConsoleColor color = ConsoleColor.Gray;
            switch (field)
            {
                case "White":
                    color = ConsoleColor.White;
                    break;
                case "Black":
                    color = ConsoleColor.Black;
                    break;
                case "Red":
                    color = ConsoleColor.Red;
                    break;
            }
            return color;
        }
    }
}
