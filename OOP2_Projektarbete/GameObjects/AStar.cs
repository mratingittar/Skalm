using Skalm.GameObjects.Interfaces;
using Skalm.Map;
using Skalm.Map.Tile;
using Skalm.Structs;

namespace Skalm.GameObjects
{
    internal class AStar
    {
        private MapManager mapManager;
        private bool debug = false;
        public AStar(MapManager mapManager)
        {
            this.mapManager = mapManager;
        }

        public Queue<Vector2Int> FindPath(BaseTile startTile, BaseTile targetTile)
        {
            if (debug)
                mapManager.mapPrinter.DrawMap();

            HashSet<BaseTile> openSet = new HashSet<BaseTile>();
            HashSet<BaseTile> closedSet = new HashSet<BaseTile>();

            openSet.Add(startTile);
            while (openSet.Count > 0)
            {
                BaseTile currentTile = openSet.MinBy(t => t.HCost) ?? openSet.First();
                openSet.Remove(currentTile);
                closedSet.Add(currentTile);

                if (currentTile == targetTile)
                    return RetracePath(startTile, targetTile);

                if (debug)
                {
                    mapManager.mapPrinter.DebugPathfinder(currentTile.GridPosition);
                    Thread.Sleep(10);
                }

                foreach (BaseTile neighbor in mapManager.GetNeighbours(currentTile.GridPosition))
                {
                    if (closedSet.Contains(neighbor) || (neighbor is ICollider && ((ICollider)neighbor).ColliderIsActive))
                        continue;

                    int newMovementCostToNeighbor = currentTile.GCost + GetDistance(currentTile, neighbor);
                    if (newMovementCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newMovementCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, targetTile);
                        neighbor.Parent = currentTile;
                    }

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
            return new Queue<Vector2Int>();
        }

        private Queue<Vector2Int> RetracePath(BaseTile startTile, BaseTile endTile)
        {
            List<BaseTile> path = new List<BaseTile>();
            BaseTile currentTile = endTile;
            while (currentTile != startTile)
            {
                path.Add(currentTile);
                currentTile = currentTile.Parent;
            }
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            path.Reverse();
            foreach (var tile in path)
            {
                queue.Enqueue(tile.GridPosition);
            }
            return queue;
        }

        private int GetDistance(BaseTile tileA, BaseTile tileB)
        {
            int distX = Math.Abs(tileA.GridPosition.X - tileB.GridPosition.X);
            int distY = Math.Abs(tileA.GridPosition.Y - tileB.GridPosition.Y);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            return 14 * distX + 10 * (distY - distX);
        }
    }
}
