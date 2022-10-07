using System.Media;

namespace Skalm.Sounds
{
    internal class SoundManager
    {
        private readonly string soundsFolderPath;
        public List<Sound> Sounds { get; private set; }
        public readonly Sound defaultSound;
        public static bool beepsOn;
        public SoundManager()
        {
            soundsFolderPath = Globals.G_SOUNDS_FOLDER_PATH;
            Sounds = InitializeSoundList();
            defaultSound = Sounds.FirstOrDefault();
            beepsOn = true;
        }

        private List<Sound> InitializeSoundList()
        {
            List<Sound> sounds = new List<Sound>();
            sounds.Add(new Sound("Steel and Seething", "Steel_and_Seething.wav"));
            sounds.Add(new Sound("Thunder Dreams", "Thunder_Dreams.wav"));

            return sounds;
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
            if (beepsOn)
                Console.Beep(frequency, duration);
        }
    }
}


//"Thunder Dreams" Kevin MacLeod(incompetech.com)
//"Steel and Seething" Kevin MacLeod(incompetech.com)
//Licensed under Creative Commons: By Attribution 4.0 License
//http://creativecommons.org/licenses/by/4.0/