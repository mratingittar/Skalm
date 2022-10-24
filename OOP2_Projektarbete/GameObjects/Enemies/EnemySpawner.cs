using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
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
            ScalingMultiplier = 1;
        }


        public Enemy Spawn(Vector2Int gridPosition, char sprite, ConsoleColor color)
        {
            return new Enemy(_mapManager, _sceneManager, new MoveIdle(), new AttackNormal(), 
                new ActorStatsObject(new StatsObject(5, 5, 5, 5, 5, 10, 1, 0), "Monster"), gridPosition, sprite, color);
        }
    }
}
