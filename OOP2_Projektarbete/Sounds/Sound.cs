namespace Skalm.Sounds
{
    internal struct Sound
    {
        public readonly string soundName;
        public readonly string fileName;

        public Sound(string soundName, string fileName)
        {
            this.soundName = soundName;
            this.fileName = fileName;
        }
    }
}
