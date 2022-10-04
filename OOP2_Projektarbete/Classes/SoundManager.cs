using System.Media;

namespace OOP2_Projektarbete.Classes
{
    internal class SoundManager
    {
        private string soundsFolderPath;

        public SoundManager()
        {
            soundsFolderPath = "..\\..\\..\\Sounds\\";
        }

        public void PlayMusic(string filename)
        {
            string path = soundsFolderPath + filename;
            if (File.Exists(path) && OperatingSystem.IsWindows())
            {
                SoundPlayer musicPlayer = new SoundPlayer(path);
                musicPlayer.Load();
                musicPlayer.PlayLooping();
            }
        }
    }
}
