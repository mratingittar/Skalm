using Skalm.Actors;
using Skalm.Actors.Tile;
using Skalm.Display;
using Skalm.Grid;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            positionsToUpdate = new Queue<Vector2Int>();

            this.tileGrid = tileGrid;


            Actor.OnPositionChanged += ActorMove;
        }

        private void ActorMove(Actor actor, Vector2Int newPos, Vector2Int oldPos)
        {
            CacheUpdatedTile(oldPos, newPos);
        }


        // METHOD CACHE UPDATED TILES
        public void CacheUpdatedTile(Vector2Int oldPos, Vector2Int newPos)
        {
            positionsToUpdate.Enqueue(oldPos);
            positionsToUpdate.Enqueue(newPos);
        }

        // METHOD REDRAW CACHED TILES
        public void RedrawCachedTiles()
        {
            Vector2Int tempPos;
            while (positionsToUpdate.Count > 0)
            {
                tempPos = positionsToUpdate.Dequeue();
                DrawSingleTile(tempPos);
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


                Console.ForegroundColor = tile.GetColor();
                foreach (var position in tileGrid.GetPlanePositions(x, y))
                {
                    printer.PrintAtPosition(tile.GetSprite(), position.Y, position.X);
                }
                Console.ForegroundColor = foregroundColor;
            }
        }
    }
}
