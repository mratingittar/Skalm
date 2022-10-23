using Skalm.GameObjects.Stats;

namespace Skalm.GameObjects.Items
{
    internal class ItemEquippable : Item
    {
        public override string Description { get => GetStatsInfo(); }
        public int equipSlot;
        public bool isDefault;

        // ITEM STATS
        public StatsObject stats;

        // CONSTRUCTOR I
        public ItemEquippable(string itemName, int equipSlot, bool isDefault = false) : base(itemName)
        {
            this.equipSlot = equipSlot;
            stats = new StatsObject(0, 0, 0, 0, 0, 0, 0, 0);
            this.isDefault = isDefault;
        }

        // CONSTRUCTOR II
        public ItemEquippable(string itemName, int equipSlot, StatsObject stats) : base(itemName)
        {
            this.equipSlot = equipSlot;
            this.stats = stats;
            isDefault = false;
            string info = GetStatsInfo();
        }

        // USE ITEM
        public override void Use(Player player)
        {
            // EQUIP ITEM
            player.equipmentManager.EquipItem(player.statsObject.stats, this);
        }

        public string GetStatsInfo()
        {
            string info = "";
            var items = stats.statsArr.Where(x => x.GetValue() > 0);
            if (items != null)
            {
                info += $"Increases your {items.First().statName}";
                if (items.Count() > 1)
                {

                    for (int i = 1; i < items.Count() - 1; i++)
                    {
                        info += $", {items.ElementAt(i).statName}";
                    }
                info += $" and {items.Last().statName}";
                }
            }
            return info;
        }


        public override int CompareTo(Item? other)
        {
            if (other == null)
                return -1;

            if (other is ItemEquippable)
                return Name.CompareTo(other.Name);
            else
                return 1;
        }
    }
}
