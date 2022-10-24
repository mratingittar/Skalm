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
        public MapGenerator MapGenerator { get; private set; }
        public MapPrinter MapPrinter { get; private set; }
        public AStar Pathfinder { get; private set; }

        // CONSTRUCTOR I
        public MapManager(ISettings settings, DisplayManager displayManager, Grid2D<BaseTile> tileGrid)
        {
            Pathfinder = new AStar(this);
            TileGrid = tileGrid;
            MapGenerator = new MapGenerator(this, displayManager, tileGrid, settings);
            MapPrinter = new MapPrinter(tileGrid, displayManager.Printer);

            Actor.OnPositionChanged += UpdateMoveablePosition;
        }

        private void UpdateMoveablePosition(Actor actor, Vector2Int newPosition, Vector2Int oldPosition)
        {
            if (TileGrid.TryGetGridObject(oldPosition, out BaseTile tileOld) && tileOld is IOccupiable tileOldOcc)
            {
                RemoveActorFromTile(actor, tileOldOcc);
            }
            if (TileGrid.TryGetGridObject(newPosition, out BaseTile tileNew) && tileNew is IOccupiable tileNewOcc)
            {
                tileNewOcc.ObjectsOnTile.Push(actor);
                tileNewOcc.ActorPresent = true;
            }
            MapPrinter.CacheUpdatedTile(oldPosition, newPosition);
        }

        private void RemoveActorFromTile(Actor actor, IOccupiable tileOldOcc)
        {
            Stack<GameObject> objects = new Stack<GameObject>();

            while (tileOldOcc.ObjectsOnTile.Count > 0 && tileOldOcc.ObjectsOnTile.Peek() != actor)
            {
                objects.Push(tileOldOcc.ObjectsOnTile.Pop());
            }
            tileOldOcc.ObjectsOnTile.TryPop(out GameObject? obj);

            while (objects.Count > 0)
            {
                tileOldOcc.ObjectsOnTile.Push(objects.Pop());
            }

            tileOldOcc.ActorPresent = false;
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

        public Vector2Int GetRandomFloorPosition()
        {
            if (MapGenerator.FloorTiles.Count == 0)
                throw new Exception("No free tiles to spawn in found");

            Random randomPos = new Random();
            return MapGenerator.FloorTiles.ElementAt(randomPos.Next(MapGenerator.FloorTiles.Count()));
        }

    }
}
