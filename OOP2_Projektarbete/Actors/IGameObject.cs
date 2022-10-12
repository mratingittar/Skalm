using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors
{
    internal interface IGameObject
    {
        //void UpdateEarly();
        void UpdateMain();
        //void UpdateLate();
    }
}
