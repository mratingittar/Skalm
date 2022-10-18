using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Interfaces;
using Skalm.Grid;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Map
{
    internal class MapManager
    {
        public Grid2D<BaseTile> TileGrid { get; private set; }
        public readonly MapGenerator mapGenerator;
        public readonly MapPrinter mapPrinter;
        private ISettings settings;
        public AStar pathfinder;

        private Queue<Actor> moveQueue;

        // CONSTRUCTOR I
        public MapManager(ISettings settings, DisplayManager displayManager, Grid2D<BaseTile> tileGrid)
        {
            pathfinder = new AStar(this);
            TileGrid = tileGrid;
            this.settings = settings;
            mapGenerator = new MapGenerator(this, tileGrid, settings);
            mapPrinter = new MapPrinter(tileGrid, displayManager.printer, settings.ForegroundColor);

            moveQueue = new Queue<Actor>();

            Actor.OnPositionChanged += UpdateMoveablePosition;
            Actor.OnMoveRequested += CheckForCollision;
        }

        private void UpdateMoveablePosition(Actor actor, Vector2Int newPosition, Vector2Int oldPosition)
        {
            TileGrid.TryGetGridObject(oldPosition, out BaseTile tileOld);
            {
                ((IOccupiable)tileOld).ObjectsOnTile.Remove(actor);
                ((IOccupiable)tileOld).ActorPresent = false;
            }
            TileGrid.TryGetGridObject(newPosition, out BaseTile tileNew);
            {
                ((IOccupiable)tileNew).ObjectsOnTile.Add(actor);
                ((IOccupiable)tileNew).ActorPresent = true;
            }
        }


        private bool CheckForCollision(Vector2Int positionToCheck)
        {
            bool collision;

            if (TileGrid.TryGetGridObject(positionToCheck, out var collider))
            {
                if (collider is ICollider)
                    collision = ((ICollider)collider).ColliderIsActive;
                else if (collider is IOccupiable)
                    collision = ((IOccupiable)collider).ActorPresent;
                else
                    collision = false;
            }
            else
                collision = false;

            return collision;
        }

        public List<BaseTile> GetNeighbours(Vector2Int tile)
        {
            List<BaseTile> neighbors = new List<BaseTile>();
            if (TileGrid.TryGetGridObject(tile.Add(new Vector2Int(0, -1)), out BaseTile up))
                neighbors.Add(up);
            if (TileGrid.TryGetGridObject(tile.Add(new Vector2Int(1, 0)), out BaseTile right))
                neighbors.Add(right);
            if (TileGrid.TryGetGridObject(tile.Add(new Vector2Int(0, 1)), out BaseTile down))
                neighbors.Add(down);
            if (TileGrid.TryGetGridObject(tile.Add(new Vector2Int(-1, 0)), out BaseTile left))
                neighbors.Add(left);

            return neighbors;
        }

        public Vector2Int GetRandomSpawnPosition()
        {
            if (mapGenerator.freeTiles.Count == 0)
                throw new Exception("No free tiles to spawn in found");

            Random randomPos = new Random();
            return mapGenerator.freeTiles.ElementAt(randomPos.Next(mapGenerator.freeTiles.Count()));
        }

    }
}
