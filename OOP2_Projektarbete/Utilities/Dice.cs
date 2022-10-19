using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Utilities
{
    internal static class Dice
    {
        static readonly Random rng = new Random();

        // ROLL ONE DIE
        public static int Roll(int sides = 6)
        {
            return rng.Next(1, sides+1);
        }

        // ROLL SEVERAL DIE
        public static int Rolls(int rolls = 1, int sides = 6)
        {
            int output = 0;

            for (int throws = 0; throws < rolls; throws++)
                output += Roll(sides);

            return output;
        }

        // ROLL DICE CHANCE
        public static bool Chance(int min, int rolls = 1, int sides = 6)
        {
            bool success = false;

            for (int throws = 0; throws < rolls; throws++)
            {
                success = Roll(sides) >= min;
                if (success)
                    break;
            }

            return success;
        }
    }
}
