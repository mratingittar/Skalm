using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Stats
{
    internal class StatsObjectHard
    {
        public Stat Strength { get; private set; }
        public Stat Dexterity { get; private set; }
        public Stat Constitution { get; private set; }
        public Stat Intelligence { get; private set; }
        public Stat Luck { get; private set; }

        // CONSTRUCTOR 1
        public StatsObjectHard(Stat strength, Stat dexterity, Stat constitution, Stat intelligence, Stat luck)
        {
            Strength = strength;
            Dexterity = dexterity;
            Constitution = constitution;
            Intelligence = intelligence;
            Luck = luck;
        }

        // CONSTRUCTOR II
        public StatsObjectHard(int strength, int dexterity, int constitution, int intelligence, int luck)
        {
            Strength = new Stat("Strength", strength);
            Dexterity = new Stat("Dexterity", dexterity);
            Constitution = new Stat("Constitution", constitution);
            Intelligence = new Stat("Intelligence", intelligence);
            Luck = new Stat("Luck", luck);
        }
    }
}
