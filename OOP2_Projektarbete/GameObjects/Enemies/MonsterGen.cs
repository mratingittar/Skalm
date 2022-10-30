using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Stats;
using Skalm.Utilities;

namespace Skalm.GameObjects.Enemies
{
    internal class MonsterGen : IWeightedGenerator<ActorStatsObject>
    {
        private readonly Random rng = new Random();
        private List<(float, (string, float))> _monsterPrefixList;
        private List<(float, (string, StatsObject))> _monsterTypeList;

        // CONSTRUCTOR I
        public MonsterGen()
        {
            // MONSTER PREFIX LIST
            _monsterPrefixList = new List<(float, (string, float))>
            {
                (0.65f, ("Weak", 0.75f)),
                (0.65f, ("Lethargic", 0.75f)),
                (0.65f, ("Dull", 0.85f)),
                (1f, ("Simple", 0.9f)),
                (1f, ("Slow", 0.9f)),
                (1f, ("Normal", 1f)),
                (1f, ("Average", 1f)),
                (1f, ("Common", 1f)),
                (1f, ("Regular", 1f)),
                (1f, ("Tough", 1.1f)),
                (0.8f, ("Clever", 1.25f)),
                (0.8f, ("Strong", 1.45f)),
                (0.8f, ("Fierce", 1.45f)),
                (0.8f, ("Vicious", 1.75f)),
                (0.7f, ("Savage", 2.65f)),
                (0.7f, ("Giant", 3.0f)),
                (0.6f, ("Diabolical", 3.0f)),
                (0.5f, ("Brutal", 4.25f)),
                (0.4f, ("Monstrous", 6.5f)),
                (0.3f, ("Leviathan", 11.5f))
            };

            // MONSTER CREATURE TYPE LIST
            _monsterTypeList = new List<(float, (string, StatsObject))>
            {
                (0.1f, ("Minotaur", new StatsObject(10,5,5,5,5,10,1,0))),
                (0.1f, ("Cockatrice", new StatsObject(5,10,5,5,5,10,1,0))),
                (0.1f, ("Troll",new StatsObject(5,5,10,5,5,10,1,0))),
                (0.1f, ("Lich", new StatsObject(5,5,5,10,5,10,1,0))),
                (0.1f, ("Wyrm", new StatsObject(5,5,5,5,10,10,1,0))),
                (0.1f, ("Ooze", new StatsObject(5,5,5,5,5,15,1,0))),
                (0.1f, ("Wraith", new StatsObject(5,5,5,5,5,10,2,0))),
                (0.1f, ("Golem", new StatsObject(5,5,5,5,5,10,1,2)))
            };
        }

        // GET ENEMY WEIGHTED RANDOM
        public ActorStatsObject GetWeightedRandom(float modifier)
        {
            List<(float, (EStats, int))> statBonusList = new List<(float, (EStats, int))>
            {
                (0.1f, (EStats.Strength, 1)),
                (0.1f, (EStats.Dexterity, 1)),
                (0.1f, (EStats.Constitution, 1)),
                (0.1f, (EStats.Intelligence, 1)),
                (0.1f, (EStats.Luck, 1)),
                (0.1f, (EStats.HP, 2)),
                (0.1f, (EStats.BaseDamage, 1)),
                (0.1f, (EStats.Armor, 1))
            };

            // RANDOMIZE PREFIX
            var monsterPrefix = WeightedRandom.WeightedRandomFromList(_monsterPrefixList);
            var monsterType = WeightedRandom.WeightedRandomFromList(_monsterTypeList);
            StatsObject monsterStats = monsterType.Item2;

            string monsterName = monsterPrefix.Item1 + " " + monsterType.Item1;
            int bonusCounter = 0;
            double addBonusChance = 1;

            // ADD BONUSES
            do
            {
                bonusCounter++;
                addBonusChance *= modifier;
            } while (rng.NextDouble() < addBonusChance);

            for (int i = 0; i < bonusCounter; i++)
            {
                var bonus = WeightedRandom.WeightedRandomFromList(statBonusList);
                monsterStats.statsArr[(int)bonus.Item1].AddValue(bonus.Item2 * (float)(1 + (rng.NextDouble() * monsterPrefix.Item2 * 0.5)));
            }

            // XP VALUE
            int xpValue = (int)Math.Ceiling(monsterPrefix.Item2 * 3.5);

            return new ActorStatsObject(monsterStats, monsterName, xpValue);
        }
    }
}
