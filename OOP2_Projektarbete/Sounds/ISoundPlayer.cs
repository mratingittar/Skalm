namespace Skalm.Sounds
{
    internal interface ISoundPlayer
    {
        bool SFXEnabled { get; set; }
        void Play();
        void Play(Sound track);
        void Play(SoundManager.SoundType soundType);
        void Stop();
    }
}
