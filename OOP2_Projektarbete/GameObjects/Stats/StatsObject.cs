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
        public Stat[] statsArr;

        // CONSTRUCTOR 1
        public StatsObject(Stat strength, Stat dexterity, Stat constitution, Stat intelligence, Stat luck, Stat hpMax, Stat baseDamage, Stat armor)
        {
            statsArr = new Stat[Enum.GetValues(typeof(EStats)).Length];

            statsArr[(int)EStats.Strength] = strength;
            statsArr[(int)EStats.Dexterity] = dexterity;
            statsArr[(int)EStats.Constitution] = constitution;
            statsArr[(int)EStats.Intelligence] = intelligence;
            statsArr[(int)EStats.Luck] = luck;

            statsArr[(int)EStats.HP] = hpMax;
            statsArr[(int)EStats.BaseDamage] = baseDamage;
            statsArr[(int)EStats.Armor] = armor;
        }

        // CONSTRUCTOR II
        public StatsObject(int strength, int dexterity, int constitution, int intelligence, int luck, int hpMax, int baseDamage, int armor)
        {
            statsArr = new Stat[Enum.GetValues(typeof(EStats)).Length];

            statsArr[(int)EStats.Strength] = new Stat(EStats.Strength, strength);
            statsArr[(int)EStats.Dexterity] = new Stat(EStats.Dexterity, dexterity);
            statsArr[(int)EStats.Constitution] = new Stat(EStats.Constitution, constitution);
            statsArr[(int)EStats.Intelligence] = new Stat(EStats.Intelligence, intelligence);
            statsArr[(int)EStats.Luck] = new Stat(EStats.Luck, luck);

            statsArr[(int)EStats.HP] = new Stat(EStats.HP, hpMax);
            statsArr[(int)EStats.BaseDamage] = new Stat(EStats.BaseDamage, baseDamage);
            statsArr[(int)EStats.Armor] = new Stat(EStats.Armor, armor);
        }
    }
}
