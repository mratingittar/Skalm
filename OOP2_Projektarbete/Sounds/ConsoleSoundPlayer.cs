
using Skalm.Utilities;
using System.Media;


namespace Skalm.Sounds
{
    internal class ConsoleSoundPlayer : ISoundPlayer
    {
        private readonly SoundPlayer musicPlayer;
        public readonly string musicFolderPath;
        public bool SFXEnabled { get; set; }

        public ConsoleSoundPlayer()
        {
            musicPlayer = new SoundPlayer();
            musicFolderPath = FileHandler.rootFolder + "/audio/";
            SFXEnabled = true;
        }

        public void Play()
        {
            if (musicPlayer.IsLoadCompleted)
                musicPlayer.PlayLooping();
        }

        public void Play(Sound sound)
        {
            string path = musicFolderPath + sound.fileName; 
            if (File.Exists(path))
            {
                musicPlayer.SoundLocation = path;
                musicPlayer.PlayLooping();
            }
        }
        public void Stop()
        {
            musicPlayer.Stop();
        }

        public void Play(SoundManager.SoundType soundType)
        {
            if (SFXEnabled is false)
                return;

            switch (soundType)
            {
                case SoundManager.SoundType.Low:
                    PlayBeep(440, 100);
                    break;
                case SoundManager.SoundType.Mid:
                    PlayBeep(880, 100);
                    break;
                case SoundManager.SoundType.High:
                    PlayBeep(1760, 100);
                    break;
            }
        }

        private void PlayBeep(int frequency, int duration)
        {
                Console.Beep(frequency, duration);
        }
    }
}
