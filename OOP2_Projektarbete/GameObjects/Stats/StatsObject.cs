

namespace Skalm.GameObjects.Stats
{
    internal class StatsObject
    {
        public Stat[] statsArr;

        // CONSTRUCTOR I
        public StatsObject()
        {
            statsArr = new Stat[Enum.GetValues(typeof(EStats)).Length];

            statsArr[(int)EStats.Strength] = new Stat(EStats.Strength, 0);
            statsArr[(int)EStats.Dexterity] = new Stat(EStats.Dexterity, 0);
            statsArr[(int)EStats.Constitution] = new Stat(EStats.Constitution, 0);
            statsArr[(int)EStats.Intelligence] = new Stat(EStats.Intelligence, 0);
            statsArr[(int)EStats.Luck] = new Stat(EStats.Luck, 0);

            statsArr[(int)EStats.HP] = new Stat(EStats.HP, 0);
            statsArr[(int)EStats.BaseDamage] = new Stat(EStats.BaseDamage, 0);
            statsArr[(int)EStats.Armor] = new Stat(EStats.Armor, 0);
        }

        // CONSTRUCTOR II
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

        // CONSTRUCTOR III
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
