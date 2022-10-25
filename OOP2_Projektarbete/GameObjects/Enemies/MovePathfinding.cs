using Skalm.GameObjects.Interfaces;
using Skalm.Map.Tile;
using Skalm.Map;
using Skalm.Structs;


namespace Skalm.GameObjects.Enemies
{
    internal class MovePathfinding : IMoveBehaviour
    {
        private MapManager _mapManager;
        private Queue<Vector2Int> _path;
        private Player _player;

        public MovePathfinding(MapManager mapManager, Player player)
        {
            _mapManager = mapManager;
            _path = new Queue<Vector2Int>();
            _player = player;
        }

        public Vector2Int MoveDirection(Vector2Int currentPosition)
        {
            FindPathToPlayer(currentPosition);
            if (_path.Count > 0)
            {
                return Vector2Int.DirectionFromTo(currentPosition, _path.Dequeue());
            }
            else 
                return Vector2Int.Zero;
        }
            private void FindPathToPlayer(Vector2Int currentPosition)
            {
                _mapManager.TileGrid.TryGetGridObject(currentPosition, out BaseTile startTile);
                _mapManager.TileGrid.TryGetGridObject(_player.GridPosition, out BaseTile targetTile);
                _path = _mapManager.Pathfinder.FindPath(startTile, targetTile);
            }
    }
}
