using Skalm.Display;
using Skalm.GameObjects;
using Skalm.Grid;
using Skalm.Map.Tile;
using Skalm.States;
using Skalm.Structs;

namespace Skalm.Map
{
    internal class MapPrinter
    {
        private IPrinter _printer;
        private Grid2D<BaseTile> _tileGrid;
        private Queue<Vector2Int> _positionsToUpdate;

        // CONSTRUCTOR I
        public MapPrinter(Grid2D<BaseTile> tileGrid, IPrinter printer)
        {
            _printer = printer;
            _tileGrid = tileGrid;
            _positionsToUpdate = new Queue<Vector2Int>();
        }

        // METHOD CACHE UPDATED TILES
        public void CacheUpdatedTile(Vector2Int oldPos, Vector2Int newPos)
        {
            _positionsToUpdate.Enqueue(oldPos);
            _positionsToUpdate.Enqueue(newPos);
        }

        public void CacheUpdatedTile(Vector2Int position)
        {
            _positionsToUpdate.Enqueue(position);
        }

        // METHOD REDRAW CACHED TILES
        public void RedrawCachedTiles()
        {
            while (_positionsToUpdate.Count > 0)
            {
                DrawSingleTile(_positionsToUpdate.Dequeue());
            }
        }

        // DRAW WHOLE MAP
        public void DrawMap()
        {
            for (int x = 0; x < _tileGrid.gridWidth; x++)
            {
                for (int y = 0; y < _tileGrid.gridHeight; y++)
                {
                        DrawSingleTile(x, y);
                }
            }
        }

        public void DebugPathfinder(Vector2Int position)
        {
            foreach (var pos in _tileGrid.GetPlanePositions(position))
            {
                _printer.PrintAtPosition('?', pos.Y, pos.X);
            }
        }

        // DRAW SINGLE TILE
        public void DrawSingleTile(Vector2Int gridPosition)
        {
            DrawSingleTile(gridPosition.X, gridPosition.Y);
        }

        public void DrawSingleTile(int x, int y)
        {
            if (_tileGrid.TryGetGridObject(x, y, out BaseTile tile))
            {
                if (tile is VoidTile)
                    return;

                foreach (var position in _tileGrid.GetPlanePositions(x, y))
                {
                    _printer.PrintAtPosition(tile.Sprite, position.Y, position.X, tile.Color);
                }
            }
        }
    }
}
