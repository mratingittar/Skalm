using Skalm.Map;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;


namespace Skalm.GameObjects.Enemies
{
    internal class Enemy : Actor
    {
        private SceneManager _sceneManager;
        private MapManager _mapManager;
        private Queue<Vector2Int> path;
        public Enemy(MapManager mapManager, SceneManager sceneManager, Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
            _mapManager = mapManager;
            _sceneManager = sceneManager;
            path = new Queue<Vector2Int>();
            Player.playerTurn += MoveEnemy;
            //FindPathToPlayer();
        }

        public void MoveEnemy()
        {
            _mapManager.mapPrinter.DrawMap();
            FindPathToPlayer();
            if (path.Count > 0)
            {
                Vector2Int dir = Vector2Int.DirectionFromTo(GridPosition, path.Dequeue());
                base.Move(dir);
            }
        }

        private void FindPathToPlayer()
        {
            _mapManager.TileGrid.TryGetGridObject(GridPosition, out BaseTile startTile);
            _mapManager.TileGrid.TryGetGridObject(_sceneManager.Player.GridPosition, out BaseTile targetTile);
            path = _mapManager.pathfinder.FindPath(startTile, targetTile);
        }

        private Vector2Int RandomDirection(Direction dir) => dir switch
        {
            Direction.Up => new Vector2Int(0, -1),
            Direction.Right => new Vector2Int(1, 0),
            Direction.Down => new Vector2Int(0, 1),
            Direction.Left => new Vector2Int(-1, 0),
            _ => throw new InvalidOperationException("Not a direction"),
        };


    }

}

