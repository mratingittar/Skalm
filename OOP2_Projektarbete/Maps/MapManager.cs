using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Interfaces;
using Skalm.Grid;
using Skalm.Maps.Tiles;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Maps
{
    internal class MapManager
    {
        public Grid2D<BaseTile> TileGrid { get; }
        public MapGenerator MapGenerator { get; }
        public MapPrinter MapPrinter { get;  }
        public AStar Pathfinder { get; }

        // CONSTRUCTOR I
        public MapManager(Grid2D<BaseTile> tileGrid, MapGenerator mapGenerator, MapPrinter mapPrinter)
        {
            TileGrid = tileGrid;
            Pathfinder = new AStar(this);
            MapGenerator = mapGenerator;
            MapPrinter = mapPrinter;
            Actor.OnPositionChanged += UpdateMoveablePosition;
        }
        public List<BaseTile> GetNeighbours(Vector2Int tile) => MapGenerator.GetNeighbours(tile);

        public Vector2Int GetRandomFloorPosition() => MapGenerator.GetRandomFloorPosition();

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
    }

    public enum EMapObjects
    {
        Enemies,
        Items,
        Keys,
        Potions
    }
}
