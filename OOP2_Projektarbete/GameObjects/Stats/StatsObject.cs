using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal class StatsObject
    {
        public Dictionary<EStats, Stat> statsDict;

        public int HPcurr;

        public string name;

        // CONSTRUCTOR 1
        public StatsObject(string name, Stat strength, Stat dexterity, Stat constitution, Stat intelligence, Stat luck, Stat hpMax, Stat baseDamage)
        {
            this.name = name;

            statsDict = new Dictionary<EStats, Stat>();

            statsDict.Add(EStats.Strength, strength);
            statsDict.Add(EStats.Dexterity, dexterity);
            statsDict.Add(EStats.Constitution, constitution);
            statsDict.Add(EStats.Intelligence, intelligence);
            statsDict.Add(EStats.Luck, luck);

            statsDict.Add(EStats.HP, hpMax);
            statsDict.Add(EStats.BaseDamage, baseDamage);

            HPcurr = (int)statsDict[EStats.HP].GetValue();
        }

        // CONSTRUCTOR II
        public StatsObject(string name, int strength, int dexterity, int constitution, int intelligence, int luck, int hpMax, int baseDamage)
        {
            this.name = name;

            statsDict = new Dictionary<EStats, Stat>();

            statsDict.Add(EStats.Strength, new Stat(EStats.Strength, strength));
            statsDict.Add(EStats.Dexterity, new Stat(EStats.Dexterity, dexterity));
            statsDict.Add(EStats.Constitution, new Stat(EStats.Constitution, constitution));
            statsDict.Add(EStats.Intelligence, new Stat(EStats.Intelligence, intelligence));
            statsDict.Add(EStats.Luck, new Stat(EStats.Luck, luck));

            statsDict.Add(EStats.HP, new Stat(EStats.HP, hpMax));
            statsDict.Add(EStats.BaseDamage, new Stat(EStats.BaseDamage, baseDamage));

            HPcurr = (int)statsDict[EStats.HP].GetValue();
        }

        // TAKE DAMAGE
        public void TakeDamage(int damage)
        {
            HPcurr -= damage;
            if (HPcurr <= 0)
                HandleDeath();
        }

        // HEAL DAMAGE
        public void HealDamage(int healAmount)
        {
            HPcurr += healAmount;
            if (HPcurr > (int)statsDict[EStats.HP].GetValue())
                HPcurr = (int)statsDict[EStats.HP].GetValue();
        }

        // HANDLE DEATH
        private void HandleDeath()
        {

        }
    }
}
