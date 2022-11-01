using Skalm.GameObjects.Items;
using Skalm.Structs;
using Skalm.Utilities;


namespace Skalm.GameObjects.Enemies
{
    internal class MonsterItemDropGen
    {
        Random rng = new Random();

        private ItemSpawner _itemSpawner;
        private PotionSpawner _potionSpawner;
        private KeySpawner _keySpawner;

        // CONSTRUCTOR I
        public MonsterItemDropGen(ItemSpawner _itemSpawner, PotionSpawner _potionSpawner, KeySpawner _keySpawner)
        {
            this._itemSpawner = _itemSpawner;
            this._potionSpawner = _potionSpawner;
            this._keySpawner = _keySpawner;
        }

        // DETERMINE ITEM DROP TYPE
        public bool DetermineItemDrop(int monsterXP)
        {
            double qualityBonus = (double)monsterXP / 100;
            double minRng = 0.33 + qualityBonus;
            return (rng.NextDouble() < minRng);
        }

        // GENERATE ITEM PICKUP
        public ItemPickup GenerateItemPickup(Vector2Int posXY, int monsterXP, float difficultyMod = 1)
        {
            // BASE ITEM DROP TYPE LIST
            List<(float, string)> itemTypeList = new List<(float, string)>
            {
                (2.5f, "potion"),
                (2.0f, "key"),
                (1.5f, "equipment")
            };

            // MONSTER XP BONUS
            float qualityBonus = (float)monsterXP / 200f;

            // MODIFY WEIGHTS
            var newList = WeightedRandom.ScaleListWeights(itemTypeList, difficultyMod + qualityBonus);

            // SELECT ITEM TYPE
            var itemType = WeightedRandom.WeightedRandomFromList(newList);

            // DROP ITEM OF DETERMINED TYPE
            switch (itemType)
            {
                // POTION
                case "potion":
                    return _potionSpawner.Spawn(posXY, 1 + qualityBonus);

                // KEY
                case "key":
                    return _keySpawner.Spawn(posXY);

                // EQUIPMENT
                case "equipment":
                    return _itemSpawner.Spawn(posXY, 1 + qualityBonus);

                // DEFAULT: POTION
                default:
                    return _potionSpawner.Spawn(posXY, 1 + qualityBonus);
            }
        }
    }
}
