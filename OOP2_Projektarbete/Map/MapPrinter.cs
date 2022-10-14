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
        MapManager mapManager;
        DisplayManager displayManager;
        IPrinter printer;
        Grid2D<BaseTile> tileGrid;
        HashSet<BaseTile> tileSet;

        private Queue<Vector2Int> positionsToUpdate;
        private ConsoleColor foregroundColor;

        // CONSTRUCTOR I
        public MapPrinter(MapManager mapManager, DisplayManager displayManager, ConsoleColor foregroundColor)
        {
            this.mapManager = mapManager;
            this.displayManager = displayManager;
            this.foregroundColor = foregroundColor;

            positionsToUpdate = new Queue<Vector2Int>();

            tileGrid = mapManager.tileGrid;
            tileSet = GetAllTiles(tileGrid);

            printer = displayManager.printer;

            Actor.OnPositionChanged += ActorMove;
        }

        private void ActorMove(Actor actor, Vector2Int newPos, Vector2Int oldPos)
        {
            CacheUpdatedTile(oldPos, newPos);
        }

        private HashSet<BaseTile> GetAllTiles(Grid2D<BaseTile> grid)
        {
            HashSet<BaseTile> set = new HashSet<BaseTile> ();
            for (int x = 0; x < grid.gridWidth; x++)
            {
                for (int y = 0; y < grid.gridHeight; y++)
                {
                    if (grid.TryGetGridObject(x, y, out BaseTile tile))
                        set.Add(tile);
                }
            }
            return set;
        }

        // METHOD CASH UPDATED TILES
        public void CacheUpdatedTile(Vector2Int oldPos, Vector2Int newPos)
        {
            positionsToUpdate.Enqueue(oldPos);
            positionsToUpdate.Enqueue(newPos);
        }

        // METHOD REDRAW CASHED TILES
        public void RedrawCachedTiles()
        {
            Vector2Int tempPos;
            while (positionsToUpdate.Count > 0)
            {
                tempPos = positionsToUpdate.Dequeue();
                DrawSingleTile(tempPos);
            }
        }

        // REDRAW WHOLE MAP
        public void RedrawMap()
        {
            foreach (BaseTile tile in tileSet)
            {
                DrawSingleTile(tile.GridPosition);
            }

            //LOOP THROUGH GRID COLUMNS &ROWS
            //for (int y = 0; y < tileGrid.gridHeight; y++)
            //{
            //    for (int x = 0; x < tileGrid.gridWidth; x++)
            //    {
                    //BaseTile tileCurr = tileGrid.GetGridObject(i, j)!;
                    //if (tileCurr.actorsAtPosition.Count > 0)
                    //    DrawSingleTile(tileCurr.actorsAtPosition[0]);
                    //else
                                //DrawSingleTile(tileGrid, i, j);
                //}
            //}

            // DRAW ACTORS
            //foreach (var actor in mapManager.actors)
            //{
            //        DrawSingleTile(actor.GridPosition);

            //}
        }

        private void DrawSingleTile(Vector2Int gridPosition)
        {
            if (tileGrid.TryGetGridObject(gridPosition, out BaseTile tile))
            {
                Console.ForegroundColor = tile.GetColor();
                foreach (var position in tileGrid.GetPlanePositions(gridPosition))
                {
                    printer.PrintAtPosition(tile.GetSprite(), position.Y, position.X);
                }
                Console.ForegroundColor = foregroundColor;
            }
        }

        // PRINT SINGLE TILE
        private void DrawSingleTile(Grid2D<BaseTile> tileGrid, int x, int y)
        {
            // GET TILE AT POSITION & TILE INFO
            BaseTile tileCurr = tileGrid.GetGridObject(x, y)!;
            char toPrint = tileCurr == null ? ' ' : tileCurr!.GetSprite();
            ConsoleColor printCol = tileCurr == null ? ConsoleColor.Gray : tileCurr.Color;

            // PRINT TO CONSOLE
            Console.ForegroundColor = printCol;
            foreach (var tile in tileGrid.GetPlanePositions(x, y))
            {
                printer.PrintAtPosition(toPrint, tile.Y, tile.X);
            }

            // RESET CONSOLE PRINT COLOR
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        // PRINT SINGLE TILE OVERLOAD
        private void DrawSingleTile(ActorTile tileCurr)
        {
            // GET TILE AT POSITION & TILE INFO
            char toPrint = tileCurr == null ? ' ' : tileCurr!.Sprite;
            ConsoleColor printCol = tileCurr == null ? ConsoleColor.Gray : tileCurr.Color;

            // PRINT TO CONSOLE
            Console.ForegroundColor = printCol;
            foreach (var tile in tileGrid.GetPlanePositions(tileCurr!.GridPosition.X, tileCurr!.GridPosition.Y))
            {
                printer.PrintAtPosition(toPrint, tile.Y, tile.X);
            }

            // RESET CONSOLE PRINT COLOR
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
