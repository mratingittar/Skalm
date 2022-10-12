using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Fighting
{
    internal interface IAttackComponent
    {
        DoDamage Attack();
    }
}
