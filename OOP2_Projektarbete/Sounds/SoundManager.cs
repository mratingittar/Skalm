using System.Media;

namespace Skalm.Sounds
{
    internal class SoundManager
    {
        private string soundsFolderPath;
        public string MenuMusic { get; private set; }

        public SoundManager()
        {
            soundsFolderPath = "..\\..\\..\\Sounds\\";
            MenuMusic = "Thunder_Dreams.wav";
        }

        public void PlayMusic(string filename)
        {
            string path = soundsFolderPath + filename;
            if (File.Exists(path))
            {
                SoundPlayer musicPlayer = new SoundPlayer(path);
                musicPlayer.Load();
                musicPlayer.PlayLooping();
            }
        }

        public static void PlayMoveBeep()
        {
            PlayBeep(440, 100);
        }
        public static void PlayConfirmBeep()
        {
            PlayBeep(880, 100);
        }
        private static void PlayBeep(int frequency, int duration)
        {
            Console.Beep(frequency, duration);
        }
    }
}
