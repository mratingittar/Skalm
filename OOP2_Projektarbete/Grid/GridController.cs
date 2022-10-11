using Skalm.Display;
using Skalm.Structs;

namespace Skalm.Grid
{
    internal class GridController
    {
        private Grid2D<Pixel> pixelGrid;
        private readonly IPrinter printer;
        private readonly IEraser eraser;
        public Dictionary<string, HashSet<Pixel>> cellsInSections;
        public Dictionary<string, Bounds> sectionRealBounds;
        public HashSet<Pixel> borderCells;

        public GridController(Grid2D<Pixel> sectionGrid, IPrinter printer, IEraser eraser)
        {
            this.pixelGrid = sectionGrid;
            this.printer = printer;
            this.eraser = eraser;
            cellsInSections = new Dictionary<string, HashSet<Pixel>>();
            borderCells = new HashSet<Pixel>();
            sectionRealBounds = new Dictionary<string, Bounds>();
        }



        public void DefineGridSections(Bounds area, Section section)
        {
            if (!cellsInSections.ContainsKey(section.GetType().Name))
                cellsInSections.Add(section.GetType().Name, new HashSet<Pixel>());

            for (int x = area.StartXY.X; x < area.EndXY.X; x++)
            {
                for (int y = area.StartXY.Y; y < area.EndXY.Y; y++)
                {
                    Pixel? cell = pixelGrid.GetGridObject(x, y);

                    if (cell != null && cell.PartOfHUD is HUDBorder)
                    {
                        pixelGrid.GridArray[x, y].PartOfHUD = section;
                        cellsInSections[section.GetType().Name].Add(cell);
                    }
                }
            }
        }

        public void DefineSectionBounds()
        {
            foreach (var section in cellsInSections.Keys)
            {
                Bounds bounds = new Bounds(cellsInSections[section].First().planePositions.First(), cellsInSections[section].Last().planePositions.Last());
                sectionRealBounds.Add(section, bounds);
            }            
        }

        public void DisplayMessage(string msg)
        {
            Bounds msgBox = sectionRealBounds["MessageSection"];
            int counter = 0;
            for (int y = msgBox.StartXY.Y + 1; y <= msgBox.EndXY.Y - 1; y++)
            {
                for (int x = msgBox.StartXY.X + 2; x <= msgBox.EndXY.X - 2; x++)
                {
                    if (counter == msg.Length)
                        return;

                    printer.PrintAtPosition(msg[counter], y, x);
                    counter++;
                }
            }
        }

        public void PrintToSection(Section section)
        {
            Vector2Int start = sectionRealBounds[section.GetType().Name].StartXY;
            Vector2Int end = sectionRealBounds[section.GetType().Name].EndXY;
            int x = start.X;
            int y = start.Y;

            for (x = start.X; x <= end.X; x++)
            {
                for (y = start.Y; y <= end.Y; y++)
                {

                    printer.PrintAtPosition('o', y, x);
                }
            }
        }

        public void FindBorderPixels()
        {
            for (int x = 0; x < pixelGrid.gridWidth; x++)
            {
                for (int y = 0; y < pixelGrid.gridHeight; y++)
                {
                    Pixel? cell = pixelGrid.GetGridObject(x, y);

                    if (cell is not null)
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



    }
}
