namespace Skalm.Utilities
{
    internal class Settings : ISettings
    {
        public string GameTitle { get; private set; } = "";
        public int UpdateFrequency { get; private set; }
        public float BaseSpawningModifier { get; private set; }

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

        public char BorderSprite { get; private set; }
        public char WallSprite { get; private set; }
        public char FloorSprite { get; private set; }
        public char DoorSpriteOpen { get; private set; }
        public char DoorSpriteClosed { get; private set; }

        public char GoalSprite { get; private set; }
        public char PlayerSprite { get; private set; }
        public char EnemySprite { get; private set; }
        public char KeySprite { get; private set; }
        public char PotionSprite { get; private set; }
        public char ItemSprite { get; private set; }

        public ConsoleColor GoalColor { get; private set; }
        public ConsoleColor PlayerColor { get; private set; }
        public ConsoleColor EnemyColor { get; private set; }
        public ConsoleColor ItemColor { get; private set; }
        public ConsoleColor PotionColor { get; private set; }
        public ConsoleColor KeyColor { get; private set; }
        public ConsoleColor HUDColor { get; private set; }
        public ConsoleColor TextColor { get; private set; }
        public ConsoleColor BackgroundColor { get; private set; }

        public virtual bool LoadSettings(string[] settingsFile)
        {
            bool success = ApplySettings(settingsFile);
            CheckForMinValues();
            return success;
        }

        private void CheckForMinValues()
        {
            StatsWidth = Math.Max(StatsWidth, 23);
            MainStatsHeight = Math.Max(MainStatsHeight, 11);
            MessageBoxHeight = Math.Max(MessageBoxHeight, 3);
        }

        private bool ApplySettings(string[] settingsFile)
        {
            foreach (var line in settingsFile)
            {
                if (line.Trim() == "" || line[0] == '#')
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
                        case "Single":
                            GetType().GetProperty(name)!.SetValue(this, ParseFloat(value));
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

        private float ParseFloat(string field)
        {
            float.TryParse(field, out float value);
            return value;
        }

        private bool ParseBool(string field)
        {
            bool.TryParse(field, out bool value);
            return value;
        }

        private ConsoleColor ParseColor(string field)
        {
            ConsoleColor color;
            if (Enum.TryParse(typeof(ConsoleColor), field, true, out var result) && result is not null)
                color = (ConsoleColor)result;
            else
                color = ConsoleColor.White;

            return color;
        }
    }
}
