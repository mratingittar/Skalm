using Skalm.GameObjects.Interfaces;
using Skalm.Map;
using Skalm.Structs;


namespace Skalm.GameObjects.Enemies
{
    internal class EnemySpawner: ISpawner<Enemy>, IScalable
    {
        public float ScalingMultiplier { get; set ; }
        private float _scaledModifier => Math.Min(_baseModifier * (1 + 0.01f * ScalingMultiplier), 0.99f);
        private float _baseModifier;
        private Player _player;
        private MonsterGen _monsterGen;
        private MapManager _mapManager;

        public EnemySpawner(float baseModifier, MapManager mapManager, Player player, MonsterGen monsterGen)
        {
            _baseModifier = baseModifier;
            _mapManager = mapManager;
            _player = player;
            _monsterGen = monsterGen;
        }

        public Enemy Spawn(Vector2Int gridPosition, char sprite, ConsoleColor color)
        {
            return new Enemy(_mapManager, _player, new MovePathfinding(_mapManager, _player), new AttackNormal(), 
                _monsterGen.GetWeightedRandom(_scaledModifier), gridPosition, sprite, color);
        }
    }
}
