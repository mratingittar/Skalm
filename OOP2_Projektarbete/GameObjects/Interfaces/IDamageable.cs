using Skalm.GameObjects.Stats;
using Skalm.Structs;


namespace Skalm.GameObjects.Interfaces
{
    internal interface IDamageable
    {
        ActorStatsObject statsObject { get; set; }
        void TakeDamage(DoDamage damage);
    }
}
