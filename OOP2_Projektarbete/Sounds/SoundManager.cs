using System.Media;

namespace Skalm.Sounds
{
    internal class SoundManager
    {
        private readonly string soundsFolderPath;
        public List<Sound> Tracks { get; private set; }
        public readonly Sound defaultSound;
        public bool beepsEnabled;
        public SoundManager()
        {
            soundsFolderPath = Globals.G_SOUNDS_FOLDER_PATH;
            Tracks = CreateSoundsList();
            Random random = new Random();
            defaultSound = Tracks[random.Next(Tracks.Count)];
            beepsEnabled = true;
        }

        private List<Sound> CreateSoundsList()
        {
            List<string> fileNames = LoadFileNamesFromFolder(soundsFolderPath);
            List<Sound> sounds = new List<Sound>();
            foreach (string fileName in fileNames)
            {
                string soundName = fileName.Replace('_', ' ').Split('.').First();
                sounds.Add(new Sound(soundName, fileName));
            }
            return sounds;
        }

        private List<string> LoadFileNamesFromFolder(string path)
        {
            string[] files = Directory.GetFiles(path, "*.wav");
            List<string> fileNames = new();
            foreach (string file in files)
                fileNames.Add(Path.GetFileName(file));
            return fileNames;
        }

        public void PlayMusic(Sound sound)
        {
            string path = soundsFolderPath + sound.fileName;
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
            if (true)
                Console.Beep(frequency, duration);
        }
    }
}


//"Thunder Dreams" Kevin MacLeod(incompetech.com)
//"Steel and Seething" Kevin MacLeod(incompetech.com)
//Licensed under Creative Commons: By Attribution 4.0 License
//http://creativecommons.org/licenses/by/4.0/