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
        public readonly AStar pathfinder;

        private ISettings _settings;

        // CONSTRUCTOR I
        public MapManager(ISettings settings, DisplayManager displayManager, Grid2D<BaseTile> tileGrid)
        {
            pathfinder = new AStar(this);
            TileGrid = tileGrid;
            _settings = settings;
            mapGenerator = new MapGenerator(this, tileGrid, settings);
            mapPrinter = new MapPrinter(tileGrid, displayManager.printer, settings.ForegroundColor);

            Actor.OnPositionChanged += UpdateMoveablePosition;
        }

        private void UpdateMoveablePosition(Actor actor, Vector2Int newPosition, Vector2Int oldPosition)
        {
            if (TileGrid.TryGetGridObject(oldPosition, out BaseTile tileOld) && tileOld is IOccupiable tileOldOcc)
            {
                Stack<GameObject> objects = new Stack<GameObject>();

                while (tileOldOcc.ObjectsOnTile.Peek() != actor)
                {
                    objects.Push(tileOldOcc.ObjectsOnTile.Pop());
                }
                tileOldOcc.ObjectsOnTile.Pop();

                while (objects.Count > 0)
                {
                    tileOldOcc.ObjectsOnTile.Push(objects.Pop());
                }

                tileOldOcc.ActorPresent = false;
            }
            if (TileGrid.TryGetGridObject(newPosition, out BaseTile tileNew) && tileNew is IOccupiable tileNewOcc)
            {
                tileNewOcc.ObjectsOnTile.Push(actor);
                tileNewOcc.ActorPresent = true;
            }
            mapPrinter.CacheUpdatedTile(oldPosition, newPosition);
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
            if (mapGenerator.FloorTiles.Count == 0)
                throw new Exception("No free tiles to spawn in found");

            Random randomPos = new Random();
            return mapGenerator.FloorTiles.ElementAt(randomPos.Next(mapGenerator.FloorTiles.Count()));
        }

    }
}
