using Skalm.GameObjects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal class EquipmentManager
    {
        Dictionary<EquipSlots, Item> equipDict;

        public EquipmentManager()
        {
            equipDict = new Dictionary<EquipSlots, Item>();
        }
    }

    public enum EquipSlots
    {
        Head,
        Torso,
        LHand,
        RHand,
        LFinger,
        RFinger,
        Feet
    }
}
