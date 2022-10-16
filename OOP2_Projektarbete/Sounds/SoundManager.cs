using Skalm.Utilities;
using System.Media;

namespace Skalm.Sounds
{
    internal class SoundManager
    {
        public ISoundPlayer player;
        public List<Sound> Tracks { get; private set; }
        public Sound CurrentlyPlaying { get; private set; }
        public SoundManager(ISoundPlayer soundPlayer)
        {
            player = soundPlayer;
            Tracks = CreateSoundsList("/audio/");
        }

        public void PlayMusic(Sound track)
        {
            CurrentlyPlaying = track;
            player.Play(track);
        }

        public void PlayRandomSong()
        {
            Random random = new Random();
            player.Play(Tracks[random.Next(Tracks.Count)]);
        }

        /// <summary>
        /// Creates a list of Sounds to play.
        /// </summary>
        /// <param name="folder">Path to sounds folder.</param>
        /// <returns></returns>
        private List<Sound> CreateSoundsList(string folder)
        {
            List<string> fileNames = LoadFileNamesFromFolder(folder);
            List<Sound> sounds = new List<Sound>();
            foreach (string fileName in fileNames)
            {
                string soundName = fileName.Replace('_', ' ').Split('.').First();
                sounds.Add(new Sound(soundName, fileName));
            }
            return sounds;
        }

        /// <summary>
        /// Loads .wav files from Sounds folder on startup and places their file names into a List.
        /// </summary>
        /// <param name="folder">Path to sounds folder.</param>
        /// <returns></returns>
        private List<string> LoadFileNamesFromFolder(string folder)
        {            
            string[] files = Directory.GetFiles(FileHandler.rootFolder + folder, "*.wav");
            List<string> fileNames = new();
            foreach (string file in files)
                fileNames.Add(Path.GetFileName(file));
            return fileNames;
        }

        /// <summary>
        /// The type of SFX to be played.
        /// </summary>
        public enum SoundType
        {
            Move,
            Confirm
        }
    }
}