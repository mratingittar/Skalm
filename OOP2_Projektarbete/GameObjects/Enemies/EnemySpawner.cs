using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Items;
using Skalm.Map;
using Skalm.Structs;


namespace Skalm.GameObjects.Enemies
{
    internal class EnemySpawner: ISpawner<Enemy>, IScalable
    {
        public float ScalingMultiplier { get; set ; }
        private float _scaledModifier => Math.Min(_baseModifier * (1 + 0.01f * ScalingMultiplier), 0.99f);
        private float _baseModifier;

        private MonsterGen _monsterGen;
        private MapManager _mapManager;
        private SceneManager _sceneManager;

        public EnemySpawner(float baseModifier, MapManager mapManager, SceneManager sceneManager, MonsterGen monsterGen)
        {
            _baseModifier = baseModifier;
            _mapManager = mapManager;
            _sceneManager = sceneManager;
            _monsterGen = monsterGen;
        }


        public Enemy Spawn(Vector2Int gridPosition, char sprite, ConsoleColor color)
        {
            return new Enemy(_mapManager, _sceneManager, new MovePathfinding(_mapManager, _sceneManager), new AttackNormal(), 
                _monsterGen.GetWeightedRandom(_scaledModifier), gridPosition, sprite, color);
        }
    }
}
