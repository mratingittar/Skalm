using Skalm.Display;
using Skalm.Structs;

namespace Skalm.Grid
{
    internal class GridController
    {
        public readonly Grid2D<Pixel> pixelGrid;
        private readonly IPrinter printer;
        private readonly IEraser eraser;
        public Dictionary<string, HashSet<Pixel>> cellsInSections;
        public HashSet<Pixel> borderCells;

        public GridController(Grid2D<Pixel> sectionGrid, IPrinter printer, IEraser eraser)
        {
            this.pixelGrid = sectionGrid;
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
                    Pixel? cell = pixelGrid.GetGridObject(x, y);

                    if (cell != null && cell.PartOfHUD is HUDBorder)
                    {
                        pixelGrid.GridArray[x, y].PartOfHUD = section;
                        cellsInSections[section.GetType().Name].Add(cell);
                    }
                }
            }
        }

        public void DisplayStats()
        {

        }

        public void DisplayMessage(string msg)
        {
            Bounds msgBox = new Bounds(pixelGrid.GetPlanePosition(cellsInSections["MessageSection"].First().gridPosition), 
                pixelGrid.GetPlanePosition(cellsInSections["MessageSection"].Last().gridPosition));
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
                foreach (var position in pixelGrid.GetPlanePositions(cell.gridPosition.X, cell.gridPosition.Y))
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
