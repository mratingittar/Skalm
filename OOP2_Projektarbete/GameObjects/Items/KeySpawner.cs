using Skalm.GameObjects.Interfaces;
using Skalm.Structs;


namespace Skalm.GameObjects.Items
{
    internal class KeySpawner : ISpawner<ItemPickup>
    {
        private char _keySprite;
        private ConsoleColor _keyColor;

        public KeySpawner(char keySprite, ConsoleColor keyColor)
        {
            _keySprite = keySprite;
            _keyColor = keyColor;
        }

        public ItemPickup Spawn(Vector2Int position, float scalingMod = 1)
        {
            return new ItemPickup(position, _keySprite, _keyColor, new Key());
        }
    }
}
