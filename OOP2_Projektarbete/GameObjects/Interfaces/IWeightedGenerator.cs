

namespace Skalm.GameObjects.Interfaces
{
    internal interface IWeightedGenerator<out T>
    {
        T GetWeightedRandom(float modifier);
    }
}
