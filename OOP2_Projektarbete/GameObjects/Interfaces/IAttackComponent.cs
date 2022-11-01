using Skalm.GameObjects.Stats;


namespace Skalm.GameObjects.Interfaces
{
    internal interface IAttackComponent
    {
        string Attack(ActorStatsObject statsAtk, ActorStatsObject statsDfn);
    }
}
