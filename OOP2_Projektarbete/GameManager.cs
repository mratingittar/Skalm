using OOP2_Projektarbete.Classes.Managers;

internal class GameManager
{
    public ManagerDisplay DisplayManager;

    public GameManager()
    {
        DisplayManager = new ManagerDisplay();
    }
}

