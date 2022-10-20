using Skalm.GameObjects.Stats;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Interfaces
{
    internal interface IDamageable
    {
        ActorStatsObject statsObject { get; set; }
        void TakeDamage(DoDamage damage);
    }
}
