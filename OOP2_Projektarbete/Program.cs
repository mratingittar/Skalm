using Skalm;
using Skalm.Display;
using Skalm.Grid;
using Skalm.Input;
using Skalm.Map;
using Skalm.Menu;
using Skalm.Sounds;
using Skalm.Structs;

#region SETTINGS
Console.WriteLine("Loading settings");
ISettings settings = new Settings();

if (!FileHandler.TryReadFile("settings.txt", out string[]? file) || !settings.LoadSettings(file!))
{
    Console.WriteLine("Settings file not found or incomplete, loading default settings");
    settings = new DefaultSettings();
    if (!settings.LoadSettings(file!))
        Console.WriteLine("Unable to create settings file, continuing with default settings");
}
Console.WriteLine("Settings loaded");
#endregion

Console.Title = settings.GameTitle;
Console.CursorVisible = settings.DisplayCursor;
Console.BackgroundColor = settings.BackgroundColor;
Console.ForegroundColor = settings.ForegroundColor;

DisplayManager displayManager = new DisplayManager(settings, new ConsoleWindowPrinter(settings.ForegroundColor, settings.BackgroundColor), new ConsoleWindowEraser(), new ConsoleWindowInfo());
SoundManager soundManager = new SoundManager(new ConsoleSoundPlayer(settings.SoundsFolderPath), settings.SoundsFolderPath);
InputManager inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());
MenuManager menuManager = new MenuManager(inputManager, displayManager, soundManager);

MapManager mapManager = new MapManager(new Grid2D<Tile>(settings.MapWidth, settings.MapHeight, settings.CellWidth, settings.CellHeight, displayManager.pixelGridController.cellsInSections["MapSection"].First().planePositions.First(), (x, y, gridPosition) => new Tile(new Vector2Int(x, y))));

GameManager game = new GameManager(settings, displayManager, mapManager, soundManager, inputManager, menuManager);
game.Start();