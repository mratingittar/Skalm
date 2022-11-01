
using Skalm.GameObjects.Stats;


namespace Skalm.Structs
{
    internal struct DoDamage
    {
        public float damage;
        public Vector2Int originXY;
        public ActorStatsObject sender;
        public bool isCritical;
    }
}
