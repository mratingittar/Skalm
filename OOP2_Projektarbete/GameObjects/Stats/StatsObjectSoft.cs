using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal class StatsObjectSoft
    {
        public int HpMax { get; private set; }
        public int HpCurrent { get; private set; }

        public int BaseDamage { get; private set; }


        // CONSTRUCTOR I
        public StatsObjectSoft(int hpMax, int baseDamage)
        {
            HpMax = hpMax;
            HpCurrent = hpMax;
            BaseDamage = baseDamage;
        }

        public void TakeDamage(int damage)
        {
            HpCurrent -= damage;
            if (HpCurrent <= 0)
                Die();
        }

        public void HealDamage(int healAmount)
        {
            HpCurrent += healAmount;
            if (HpCurrent > HpMax)
                HpCurrent = HpMax;
        }

        private void Die()
        {

        }
    }
}
