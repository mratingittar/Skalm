using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Stats
{
    internal class StatsObjectSoft
    {
        public int hpMax { get; private set; }
        public int hpCurr { get; private set; }

        public Stat moveSpd { get; private set; }

        // CONSTRUCTOR I
        public StatsObjectSoft(int hpMax, float moveSpd)
        {
            this.hpMax = hpMax;
            this.hpCurr = hpMax;
            this.moveSpd = new Stat("Move Speed", moveSpd);
        }
    }
}
