using Skalm.GameObjects.Stats;
using Skalm.Map;
using Skalm.Structs;


namespace Skalm.GameObjects.Enemies
{
    internal class EnemySpawner
    {
        private MapManager mapManager;
        private SceneManager sceneManager;

        public EnemySpawner(MapManager mapManager, SceneManager sceneManager)
        {
            this.mapManager = mapManager;
            this.sceneManager = sceneManager;
        }

        public Enemy Spawn(Vector2Int gridPosition, char sprite, ConsoleColor color)
        {
            return new Enemy(mapManager, sceneManager, new MoveIdle(), new AttackNormal(), 
                new ActorStatsObject(new StatsObject(5, 5, 5, 5, 5, 10, 1, 0), "Monster"), gridPosition, sprite, color);
        }
    }
}
