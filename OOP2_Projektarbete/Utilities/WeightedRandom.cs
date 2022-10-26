using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Utilities
{
    internal static class WeightedRandom
    {
        private static Random rng = new Random();
        public static T WeightedRandomFromList<T>(List<(float, T)> inList)
        {
            float sumTotal = 0;
            float sumRng = 0;
            float choice;

            // GET SUM OF ALL ITEM WEIGHTS
            foreach (var item in inList)
            {
                sumTotal += item.Item1;
            }

            // RANDOMIZE CHOICE
            choice = (float)rng.NextDouble() * sumTotal;

            // ITERATE LIST & PICK RANDOM ITEM
            for (int i = 0; i < inList.Count; i++)
            {
                sumRng += inList[i].Item1;

                // PICK ITEM IF RNG VALUE EXCEEDED
                if (choice > sumRng - inList[i].Item1
                && choice < sumRng)
                    return inList[i].Item2;
            }

            // UGLY DEFAULT
            return inList[0].Item2;
        }
    }
}
