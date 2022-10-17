using Skalm.Actors;
using Skalm.Actors.Spawning;
using Skalm.Actors.Tile;
using Skalm.Display;
using Skalm.Grid;
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
        public List<IGameObject> gameObjects { get; private set; }
        public List<Actor> actors { get; private set; }
        private Queue<Actor> moveQueue;

        // CONSTRUCTOR I
        public MapManager(ISettings settings, DisplayManager displayManager, Grid2D<BaseTile> tileGrid)
        {
            pathfinder = new AStar(this);
            TileGrid = tileGrid;
            this.settings = settings;
            mapGenerator = new MapGenerator(tileGrid, settings);
            mapPrinter = new MapPrinter(tileGrid, displayManager.printer, settings.ForegroundColor);

            gameObjects = new List<IGameObject>();
            actors = new List<Actor>();

            moveQueue = new Queue<Actor>();

            Actor.OnPositionChanged += UpdateMoveablePosition;
            Actor.OnMoveRequested += CheckForCollision;
        }

        private void UpdateMoveablePosition(Actor actor, Vector2Int newPosition, Vector2Int oldPosition)
        {
            TileGrid.TryGetGridObject(oldPosition, out BaseTile tileOld);
                ((IOccupiable)tileOld).ObjectsOnTile.Remove(actor);
            TileGrid.TryGetGridObject(newPosition, out BaseTile tileNew);
                ((IOccupiable)tileNew).ObjectsOnTile.Add(actor);
        }


        private bool CheckForCollision(Vector2Int positionToCheck)
        {
            bool collision;

            if (TileGrid.TryGetGridObject(positionToCheck, out var collider))
            {
                if (collider is ICollider)
                    collision = ((ICollider)collider).ColliderIsActive;
                else
                    collision = false;
            }
            else
                collision = false;

            return collision;
        }

        public void AddActorsToMap()
        {
            foreach (var actor in actors)
            {
                TileGrid.TryGetGridObject(actor.GridPosition, out BaseTile tile);
                ((IOccupiable)tile).ObjectsOnTile.Add(actor);
            }
        }

        public void ResetMap()
        {
            actors.Clear();
            gameObjects.Clear();
        }
    }
}
