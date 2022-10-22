using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal class Stat
    {
        public EStats statName { get; private set; }
        private float baseValue;

        // MODIFIER LIST
        private List<float> modifiers;

        // CONSTRUCTOR I
        public Stat(EStats statName, float baseValue)
        {
            this.statName = statName;
            this.baseValue = baseValue;

            modifiers = new List<float>();
        }

        // GET STAT VALUE
        public int GetValue()
        {
            float output = baseValue;
            modifiers.ForEach(x => output += x);
            return Convert.ToInt32(output);
        }

        // SET STAT VALUE
        public void SetValue(int value)
        {
            baseValue = value;
        }

        // ADD MODIFIER
        public void AddModifier(float value)
        {
            if (value > 0)
                modifiers.Add(value);
        }

        // REMOVE MODIFIER
        public void RemoveModifier(float value)
        {
            if (value > 0)
                modifiers.Remove(value);
        }
    }

    // STATS ENUM
    public enum EStats
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Luck,
        HP,
        BaseDamage,
        Armor
    }
}
