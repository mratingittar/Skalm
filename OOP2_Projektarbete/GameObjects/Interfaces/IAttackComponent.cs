using Skalm.GameObjects.Stats;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Interfaces
{
    internal interface IAttackComponent
    {
        string Attack(ActorStatsObject statsAtk, ActorStatsObject statsDfn);
    }
}
