using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal class Stat
    {
        public string name { get; private set; }
        private float baseValue;

        private List<float> modifiers;

        // CONSTRUCTOR I
        public Stat(string name, float baseValue)
        {
            this.name = name;
            this.baseValue = baseValue;

            modifiers = new List<float>();
        }

        // GET STAT VALUE
        public float GetValue()
        {
            float output = baseValue;
            modifiers.ForEach(x => output += x);
            return output;
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
}
