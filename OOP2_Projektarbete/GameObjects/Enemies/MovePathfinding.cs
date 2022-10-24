using Skalm.GameObjects.Interfaces;
using Skalm.Map.Tile;
using Skalm.Map;
using Skalm.Structs;


namespace Skalm.GameObjects.Enemies
{
    internal class MovePathfinding : IMoveBehaviour
    {
        private MapManager mapManager;
        private SceneManager sceneManager;
        private Queue<Vector2Int> path;

        public MovePathfinding(MapManager mapManager, SceneManager sceneManager)
        {
            this.mapManager = mapManager;
            this.sceneManager = sceneManager;
            path = new Queue<Vector2Int>();
        }

        public Vector2Int MoveDirection(Vector2Int currentPosition)
        {
            FindPathToPlayer(currentPosition);
            if (path.Count > 0)
            {
                return Vector2Int.DirectionFromTo(currentPosition, path.Dequeue());
            }
            else 
                return Vector2Int.Zero;
        }
            private void FindPathToPlayer(Vector2Int currentPosition)
            {
                mapManager.TileGrid.TryGetGridObject(currentPosition, out BaseTile startTile);
                mapManager.TileGrid.TryGetGridObject(sceneManager.Player.GridPosition, out BaseTile targetTile);
                path = mapManager.Pathfinder.FindPath(startTile, targetTile);
            }
    }
}
