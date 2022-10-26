namespace Skalm.Utilities
{
    internal interface ISettings
    {
        string GameTitle { get; }
        int UpdateFrequency { get; }
        float BaseSpawningModifier { get; }

        int WindowPadding { get; }
        int BorderThickness { get; }
        int CellHeight { get; }
        int CellWidth { get; }
        int HudPadding { get; }
        int MainStatsHeight { get; }
        int StatsWidth { get; }
        int MapHeight { get; }
        int MapWidth { get; }
        int MessageBoxHeight { get; }

        char BorderSprite { get; }
        char DoorSpriteClosed { get; }
        char DoorSpriteOpen { get; }
        char FloorSprite { get; }
        char WallSprite { get; }

        char GoalSprite { get; }
        char PlayerSprite { get; }
        char EnemySprite { get; }
        char KeySprite { get; }
        char PotionSprite { get; }
        char ItemSprite { get; }

        ConsoleColor GoalColor { get; }
        ConsoleColor PlayerColor { get; }
        ConsoleColor EnemyColor { get; }
        ConsoleColor ItemColor { get; }
        ConsoleColor PotionColor { get; }
        ConsoleColor KeyColor { get; }
        ConsoleColor HUDColor { get; }
        ConsoleColor TextColor { get; }
        ConsoleColor BackgroundColor { get; }

        bool LoadSettings(string[] settingsFile);
    }
}