namespace Skalm
{
    internal interface ISettings
    {
        int BorderThickness { get; }
        int CellHeight { get; }
        int CellWidth { get; }
        bool DisplayCursor { get; }
        string GameTitle { get; }
        int HudPadding { get; }
        int MainStatsHeight { get; }
        int MapHeight { get; }
        int MapWidth { get; }
        int MessageBoxHeight { get; }
        string SoundsFolderPath { get; }
        char SpriteBorder { get; }
        char SpriteDoor { get; }
        char SpriteFloor { get; }
        char SpriteWall { get; }
        int StatsWidth { get; }
        int SubStatsHeight { get; }
        int UpdateFrequency { get; }
        int WindowPadding { get; }

        ConsoleColor foregroundColor { get; }
        ConsoleColor backgroundColor { get; }

        bool LoadSettings(string[] settingsFile);
    }
}