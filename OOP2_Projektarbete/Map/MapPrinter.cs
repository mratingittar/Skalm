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
        private IPrinter printer;
        private Grid2D<BaseTile> tileGrid;
        private Queue<Vector2Int> positionsToUpdate;
        private ConsoleColor foregroundColor;

        // CONSTRUCTOR I
        public MapPrinter(Grid2D<BaseTile> tileGrid, IPrinter printer, ConsoleColor foregroundColor)
        {
            this.printer = printer;
            this.foregroundColor = foregroundColor;
            this.tileGrid = tileGrid;

            positionsToUpdate = new Queue<Vector2Int>();

        }


        // METHOD CACHE UPDATED TILES
        public void CacheUpdatedTile(Vector2Int oldPos, Vector2Int newPos)
        {
            positionsToUpdate.Enqueue(oldPos);
            positionsToUpdate.Enqueue(newPos);
        }

        public void CacheUpdatedTile(Vector2Int position)
        {
            positionsToUpdate.Enqueue(position);
        }

        // METHOD REDRAW CACHED TILES
        public void RedrawCachedTiles()
        {
            while (positionsToUpdate.Count > 0)
            {
                DrawSingleTile(positionsToUpdate.Dequeue());
            }
        }

        // DRAW WHOLE MAP
        public void DrawMap()
        {
            for (int x = 0; x < tileGrid.gridWidth; x++)
            {
                for (int y = 0; y < tileGrid.gridHeight; y++)
                {
                        DrawSingleTile(x, y);
                }
            }
        }

        public void DebugPathfinder(Vector2Int position)
        {
            foreach (var pos in tileGrid.GetPlanePositions(position))
            {
                printer.PrintAtPosition('?', pos.Y, pos.X);
            }
        }

        // DRAW SINGLE TILE
        public void DrawSingleTile(Vector2Int gridPosition)
        {
            DrawSingleTile(gridPosition.X, gridPosition.Y);
        }

        public void DrawSingleTile(int x, int y)
        {
            if (tileGrid.TryGetGridObject(x, y, out BaseTile tile))
            {
                if (tile is VoidTile)
                    return;


                //Console.ForegroundColor = tile.Color;
                foreach (var position in tileGrid.GetPlanePositions(x, y))
                {
                    printer.PrintAtPosition(tile.Sprite, position.Y, position.X, tile.Color);
                }
                //Console.ForegroundColor = foregroundColor;
            }
        }
    }
}
