using Skalm.Display;
using Skalm.Structs;

namespace Skalm.Grid
{
    internal class GridController
    {
        private Grid2D<Pixel> sectionGrid;
        private IPrinter printer;
        private IEraser eraser;
        private Dictionary<string, HashSet<Pixel>> cellsInSections;
        private HashSet<Pixel> borderCells;

        public GridController(Grid2D<Pixel> sectionGrid, IPrinter printer, IEraser eraser)
        {
            this.sectionGrid = sectionGrid;
            this.printer = printer;
            this.eraser = eraser;
            cellsInSections = new Dictionary<string, HashSet<Pixel>>();
            borderCells = new HashSet<Pixel>();
        }



        public void DefineGridSections(Bounds area, Section section)
        {
            if (!cellsInSections.ContainsKey(section.GetType().Name))
                cellsInSections.Add(section.GetType().Name, new HashSet<Pixel>());

            for (int x = area.StartXY.X; x < area.EndXY.X; x++)
            {
                for (int y = area.StartXY.Y; y < area.EndXY.Y; y++)
                {
                    Pixel? cell = sectionGrid.GetGridObject(x, y);

                    if (cell != null && cell.PartOfHUD is HUDBorder)
                    {
                        sectionGrid.GridArray[x, y].PartOfHUD = section;
                        cellsInSections[section.GetType().Name].Add(cell);
                    }
                }
            }
        }

        public void FindBorderCells()
        {
            for (int x = 0; x < sectionGrid.gridWidth; x++)
            {
                for (int y = 0; y < sectionGrid.gridHeight; y++)
                {
                    Pixel? cell = sectionGrid.GetGridObject(x, y);

                    if (cell != null)
                    {
                        if (cell.PartOfHUD is HUDBorder)
                            borderCells.Add(cell);
                    }
                }
            }
        }

        public void PrintBorders()
        {
            foreach (Pixel cell in borderCells)
            {
                foreach (var position in cell.planePositions)
                {
                    printer.PrintAtPosition((cell.PartOfHUD as HUDBorder)!.borderCharacter, position.Y, position.X);
                }
            }
        }

        //public void FillSection(Section section, char chars)
        //{
        //    foreach (Cell cell in cellsInSections[section])
        //    {
        //        printer.PrintAtPosition(chars, cell.ConsolePositions[0].Y, cell.ConsolePositions[0].X);
        //    }
        //}



        //public void PrintGridContent(IContentType content) // MAYBE FIND WAY TO ONLY PASS TYPE AS PARAMETER
        //{
        //    for (int x = 0; x < gameGrid.GridArray.GetLength(0); x++)
        //    {
        //        for (int y = 0; y < gameGrid.GridArray.GetLength(1); y++)
        //        {
        //            if (gameGrid.GridArray[x, y].PartOfHUD.GetType() == content.GetType())
        //                PrintCell(gameGrid.GridArray[x, y].ConsolePositions, gameGrid.GridArray[x, y].PartOfHUD);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Prints content character to every cell in grid.
        ///// </summary>
        ///// <param name="fullGrid">Covers every console cell if true.</param>
        //public void PrintGridContent(bool fullGrid = true)
        //{
        //    for (int x = 0; x < gameGrid.GridArray.GetLength(0); x++)
        //    {
        //        for (int y = 0; y < gameGrid.GridArray.GetLength(1); y++)
        //        {
        //            if (fullGrid)
        //            {
        //                PrintCell(gameGrid.GridArray[x, y].ConsolePositions, gameGrid.GridArray[x, y].PartOfHUD.Character);
        //            }
        //            else
        //            {
        //                Vector2Int pos = gameGrid.GridArray[x, y].ConsolePositions.First();
        //                printer.PrintAtPosition(gameGrid.GridArray[x, y].PartOfHUD.Character, pos.Y, pos.X);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Prints content character to all console cells in list.
        ///// </summary>
        ///// <param name="consolePositions"></param>
        ///// <param name="character"></param>
        //private void PrintCell(List<Vector2Int> consolePositions, char character)
        //{
        //    foreach (Vector2Int position in consolePositions)
        //    {
        //        printer.PrintAtPosition(character, position.Y, position.X);
        //    }
        //}
    }
}
