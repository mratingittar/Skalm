using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Enemies
{
    internal class MonsterGen : IWeightedGenerator<Enemy>
    {
        private readonly Random rng = new Random();

        public Enemy GetWeightedRandom()
        {
            throw new NotImplementedException();
        }
    }
}
