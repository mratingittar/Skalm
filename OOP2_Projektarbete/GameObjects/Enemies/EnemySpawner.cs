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
            Console.WriteLine("Enemy Spawned");
            return new Enemy(mapManager, sceneManager, new MovePathfinding(mapManager, sceneManager), gridPosition, sprite, color);
        }
    }
}
