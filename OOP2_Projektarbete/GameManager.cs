using OOP2_Projektarbete.Classes.Managers;
using OOP2_Projektarbete.Classes.Map;

internal class GameManager
{
    public WindowManagerConsole displayManager;
    public DisplayManagerGameWindow displayManagerGameWindow;
    public MapManager mapManager;

    public GameManager()
    {
        mapManager = new MapManager();
        displayManager = new WindowManagerConsole();
        displayManagerGameWindow = new DisplayManagerGameWindow(displayManager.gameWindowBounds, mapManager);
    }
}

