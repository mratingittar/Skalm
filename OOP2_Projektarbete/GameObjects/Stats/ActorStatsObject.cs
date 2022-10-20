﻿using Skalm.GameObjects.Interfaces;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal class ActorStatsObject : IDamageable
    {
        // HARD STATS
        public StatsObject stats;

        // SOFT STATS
        public string name;
        int HPcurr;

        // CONSTRUCTOR I
        public ActorStatsObject(StatsObject stats, string name)
        {
            this.stats = stats;

            this.name = name;
            HPcurr = stats.statsArr[(int)EStats.HP].GetValue();
        }

        // GET CURRENT HP
        public int GetCurrentHP() => HPcurr;

        // TAKE DAMAGE
        public void TakeDamage(DoDamage damage)
        {
            HPcurr -= (int)Math.Round(damage.damage);
            if (HPcurr <= 0)
                HandleDeath();
        }

        // HEAL DAMAGE
        public void HealDamage(int healAmount)
        {
            HPcurr += healAmount;
            if (HPcurr > stats.statsArr[(int)EStats.HP].GetValue())
                HPcurr = stats.statsArr[(int)EStats.HP].GetValue();
        }

        // HANDLE DEATH
        private void HandleDeath()
        {

        }
    }
}