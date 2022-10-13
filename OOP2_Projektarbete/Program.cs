using Skalm;
using Skalm.Actors.Tile;
using Skalm.Display;
using Skalm.Grid;
using Skalm.Input;
using Skalm.Map;
using Skalm.Menu;
using Skalm.Sounds;
using Skalm.Structs;
using Skalm.Utilities;
using System.Reflection;

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

#region CALCULATING BOUNDS
int verticalBorders = 3;
int horizontalBorders = 3;

Rectangle gridMapRect = new Rectangle(settings.MapWidth, settings.MapHeight);
Rectangle gridMessageRect = new Rectangle(settings.MapWidth, settings.MessageBoxHeight);
Rectangle gridMainStatsRect = new Rectangle(settings.StatsWidth, settings.MainStatsHeight);
Rectangle gridSubStatsRect = new Rectangle(settings.StatsWidth, settings.MapHeight + settings.MessageBoxHeight - settings.MainStatsHeight);
Rectangle gridRect = new Rectangle(gridMapRect.Width + gridMainStatsRect.Width + settings.BorderThickness * verticalBorders,
    gridMapRect.Height + gridMessageRect.Height + settings.BorderThickness * horizontalBorders);
Rectangle consoleRect = new Rectangle((gridRect.Width + settings.WindowPadding * 2) * settings.CellWidth, (gridRect.Height + settings.WindowPadding * 2) * settings.CellHeight);

Bounds mapBounds = new Bounds(new Vector2Int(settings.BorderThickness, settings.BorderThickness), gridMapRect);
Bounds messageBounds = new Bounds(new Vector2Int(settings.BorderThickness, mapBounds.EndXY.Y + settings.BorderThickness), gridMessageRect);
Bounds mainStatsBounds = new Bounds(new Vector2Int(mapBounds.EndXY.X + settings.BorderThickness, mapBounds.StartXY.Y), gridMainStatsRect);
Bounds subStatsBounds = new Bounds(new Vector2Int(mainStatsBounds.StartXY.X, mainStatsBounds.EndXY.Y + settings.BorderThickness), gridSubStatsRect);
Dictionary<string, Bounds> sectionBounds = new Dictionary<string, Bounds> 
{
    { "mapBounds", mapBounds },
    { "messageBounds", messageBounds },
    { "mainStatsBounds", mainStatsBounds },
    { "subStatsBounds", subStatsBounds }
};
#endregion

IPrinter printer = new ConsoleWindowPrinter(settings.ForegroundColor, settings.BackgroundColor);
IEraser eraser = new ConsoleWindowEraser();

#region CREATING MANAGERS
DisplayManager displayManager = new DisplayManager(settings, printer, eraser, new ConsoleWindowInfo(), consoleRect,
    new PixelController(new Grid2D<Pixel>(gridRect.Width, gridRect.Height, settings.CellWidth, settings.CellHeight,
                new Vector2Int(settings.WindowPadding * settings.CellWidth, settings.WindowPadding * settings.CellHeight),
                (gridX, gridY, consolePositions) => new Pixel(new(gridX, gridY), new HUDBorder(settings.SpriteBorder))), sectionBounds, printer, eraser));
SoundManager soundManager = new SoundManager(new ConsoleSoundPlayer(settings.SoundsFolderPath), settings.SoundsFolderPath);
InputManager inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());
MenuManager menuManager = new MenuManager(inputManager, displayManager, soundManager);
MapManager mapManager = new MapManager(new Grid2D<BaseTile>(settings.MapWidth, settings.MapHeight, settings.CellWidth, settings.CellHeight, 
    displayManager.pixelGridController.pixelGrid.GetPlanePosition(displayManager.pixelGridController.pixelsInSections["MapSection"].First().gridPosition), 
    (x, y, gridPosition) => new VoidTile(new Vector2Int(x, y))), displayManager);

GameManager game = new GameManager(settings, displayManager, mapManager, soundManager, inputManager, menuManager);
#endregion

game.Start();