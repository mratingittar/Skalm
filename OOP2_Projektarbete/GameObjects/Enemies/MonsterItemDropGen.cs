using Skalm.GameObjects.Items;
using Skalm.Structs;
using Skalm.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return (rng.NextDouble() < 0.4);
        }

        // GENERATE ITEM PICKUP
        public void GenerateItemPickup(Vector2Int posXY, int monsterXP, float difficultyMod = 1)
        {
            Item item;
            // BASE ITEM DROP TYPE LIST
            List<(float, string)> itemTypeList = new List<(float, string)>
            {
                (3.0f, "potion"),
                (2.0f, "key"),
                (1.0f, "equipment")
            };

            // MODIFY WEIGHTS
            var newList = WeightedRandom.ScaleListWeights(itemTypeList, difficultyMod);

            // SELECT ITEM TYPE
            var itemType = WeightedRandom.WeightedRandomFromList(newList);

            // DROP ITEM OF DETERMINED TYPE
            switch (itemType)
            {
                case "potion":
                    _potionSpawner.Spawn(posXY);
                    break;

                case "key":
                    _keySpawner.Spawn(posXY);
                    break;

                case "equipment":
                    _itemSpawner.Spawn(posXY);
                    break;

                default:
                    _potionSpawner.Spawn(posXY);
                    break;
            }
        }
    }
}
