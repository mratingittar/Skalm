using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects
{
    internal interface IWeightedGenerator<out T>
    {
        T GetWeightedRandom(float modifier);
    }
}
