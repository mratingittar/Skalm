using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using Skalm.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal class ActorStatsObject
    {
        // HARD STATS
        public StatsObject stats;
        public int XpTarget { get => _xpTarget; }
        
        // SOFT STATS
        public string name;
        public int Experience;
        public int Level;

        private int _xpTarget;
        private int _hpCurrent;

        public event Action? OnStatsChanged;
        public event Action? OnDeath;

        // CONSTRUCTOR I
        public ActorStatsObject(StatsObject stats, string name, int experience)
        {
            // STATS
            this.name = name;
            this.stats = stats;
            ResetHP();

            // XP & LEVELS
            Experience = experience;
            Level = 1;
            _xpTarget = 15;
        }

        // HANDLE DEATH
        private void HandleDeath() => OnDeath?.Invoke();

        // GET CURRENT HP
        public int GetCurrentHP() => _hpCurrent;

        // RESET HP
        public void ResetHP() => _hpCurrent = GetMaxHP();

        // GET MAX HP
        public int GetMaxHP()
        {
            float statsHP = stats.statsArr[(int)EStats.HP].GetValue();
            float statsCon = stats.statsArr[(int)EStats.Constitution].GetValue();
            return (int)(statsHP + (statsCon * 1.33));
        }

        // TAKE DAMAGE
        public void TakeDamage(DoDamage damage)
        {
            _hpCurrent -= (int)Math.Round(damage.damage);
            OnStatsChanged?.Invoke();
            if (_hpCurrent <= 0)
                HandleDeath();
        }

        // HEAL DAMAGE
        public void HealDamage(int healAmount)
        {
            _hpCurrent += healAmount;
            if (_hpCurrent > GetMaxHP())
                _hpCurrent = GetMaxHP();
            OnStatsChanged?.Invoke();
        }

        // INCREASE EXPERIENCE
        public void IncreaseExperience(int gain)
        {
            Experience += gain;
            if (Experience >= _xpTarget)
                LevelUp();
            OnStatsChanged?.Invoke();
        }

        // HANDLE LEVEL UP
        private void LevelUp() 
        {
            Level++;
            Experience = Experience - _xpTarget;
            _xpTarget += (int)(_xpTarget * 1.75f);
            IncreaseRandomStat(2);
        }

        // INCREASE RANDOM STAT
        private void IncreaseRandomStat(int amount)
        {
            int stat = Dice.Roll(5) - 1;
            stats.statsArr[stat].SetValue(stats.statsArr[stat].GetValue() + amount);
        }
    }
}
