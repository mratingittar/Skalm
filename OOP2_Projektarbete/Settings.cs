namespace Skalm
{
    internal class Settings
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
        public int SubStatsHeight { get; private set; }
        public int HudPadding { get; private set; }

        public char SpriteBorder { get; private set; }
        public char SpriteWall { get; private set; }
        public char SpriteFloor { get; private set; }
        public char SpriteDoor { get; private set; }

        public string SoundsFolderPath { get; private set; } = "";


        public virtual bool LoadSettings(string[] settingsFile)
        {
            return ApplySettings(settingsFile);
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

                try
                {
                    switch (type)
                    {
                        case "int":
                        case "Int":
                        case "Int32":
                        case "int32":
                            this.GetType().GetProperty(name)!.SetValue(this, ParseInt(name, settingsFile));
                            break;
                        case "char":
                        case "Char":
                            this.GetType().GetProperty(name)!.SetValue(this, ParseChar(name, settingsFile));
                            break;
                        case "string":
                        case "String":
                            this.GetType().GetProperty(name)!.SetValue(this, ParseString(name, settingsFile));
                            break;
                        case "bool":
                        case "Bool":
                        case "Boolean":
                            this.GetType().GetProperty(name)!.SetValue(this, ParseBool(name, settingsFile));
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

        private string ParseString(string field, string[] file)
        {
            string setting = ExtractField(field, file);
            return setting.Trim();
        }

        private char ParseChar(string field, string[] file)
        {
            string setting = ExtractField(field, file);
            return setting.Trim().ToCharArray().SingleOrDefault();
        }

        private int ParseInt(string field, string[] file)
        {
            string setting = ExtractField(field, file);
            int.TryParse(setting.Trim(), out int value);
            return value;
        }

        private bool ParseBool(string field, string[] file)
        {
            string setting = ExtractField(field, file);
            bool.TryParse(setting.Trim(), out bool value);
            return value;
        }

        // Each step, remove the line from the file so it has fewer lines to check each time

        private string ExtractField(string field, string[] settings)
        {
            return settings.Where(s => s.Contains(field)).First().Split('=').Last();
        }
    }
}
