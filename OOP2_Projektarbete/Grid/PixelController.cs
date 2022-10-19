using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Stats;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Grid
{
    internal class PixelController
    {
        public readonly Grid2D<Pixel> pixelGrid;
        private readonly IPrinter printer;
        private readonly IEraser eraser;
        public Dictionary<string, Bounds> sectionBounds { get; private set; }
        public Dictionary<string, HashSet<Pixel>> pixelsInSections;
        public HashSet<Pixel> borderCells;

        private readonly Bounds messageConsole;
        private readonly Bounds mainStatsConsole;
        private readonly Bounds subStatsConsole;

        public PixelController(Grid2D<Pixel> sectionGrid, Dictionary<string, Bounds> sectionBounds, IPrinter printer, IEraser eraser)
        {
            this.pixelGrid = sectionGrid;
            this.sectionBounds = sectionBounds;
            this.printer = printer;
            this.eraser = eraser;
            pixelsInSections = new Dictionary<string, HashSet<Pixel>>();
            borderCells = new HashSet<Pixel>();

            Player.playerStats += DisplayStats;
            Player.playerInventory += DisplayInventory;

            DefineGridSections(sectionBounds["mapBounds"], new MapSection());
            DefineGridSections(sectionBounds["messageBounds"], new MessageSection());
            DefineGridSections(sectionBounds["mainStatsBounds"], new MainStatsSection());
            DefineGridSections(sectionBounds["subStatsBounds"], new SubStatsSection());

            FindBorderPixels();

            Bounds message = FindConsoleBoundsOfSection("MessageSection");
            messageConsole = new Bounds(new Vector2Int(message.StartXY.X + 2, message.StartXY.Y + 1), new Vector2Int(message.EndXY.X - 2, message.EndXY.Y - 1));

            Bounds mainStats = FindConsoleBoundsOfSection("MainStatsSection");
            mainStatsConsole = new Bounds(new Vector2Int(mainStats.StartXY.X + 2, mainStats.StartXY.Y + 1), new Vector2Int(mainStats.EndXY.X - 2, mainStats.EndXY.Y - 1));

            Bounds subStats = FindConsoleBoundsOfSection("SubStatsSection");
            subStatsConsole = new Bounds(new Vector2Int(subStats.StartXY.X + 2, subStats.StartXY.Y + 1), new Vector2Int(subStats.EndXY.X - 2, subStats.EndXY.Y - 1));
        }

        public void DisplayMessage(string msg)
        {
            PrintWithinBounds(msg, messageConsole);
        }

        public void DisplayStats(StatsObjectHard hardStats, StatsObjectSoft softStats)
        {
            string name = hardStats.Name;
            if (name.Length > mainStatsConsole.Size.Width)
                name = name.Remove(mainStatsConsole.Size.Width);

            int column = mainStatsConsole.StartXY.X;
            int row = mainStatsConsole.StartXY.Y;

            printer.PrintFromPosition(name, row, column);
            row++;
            printer.PrintFromPosition(TextTools.RepeatChar('─', name.Length), row, column);
            row++;
            printer.PrintFromPosition($"Lives: {softStats.HpCurrent} / {softStats.HpMax}", row, column);
            row++;
            printer.PrintFromPosition($"Base damage: {softStats.BaseDamage}", row, column);
            row += 2;
            printer.PrintFromPosition($"Str {hardStats.Strength.GetValue()} | Dex {hardStats.Dexterity.GetValue()} " +
                $"| Con {hardStats.Constitution.GetValue()} | Int {hardStats.Strength.GetValue()} | Lck {hardStats.Strength.GetValue()}", row, column);
        }

        public void DisplayInventory()
        {
            int column = subStatsConsole.StartXY.X;
            int row = subStatsConsole.StartXY.Y;

            printer.PrintFromPosition("Equipment", row, column);
            row++;
            printer.PrintFromPosition("─────────", row, column);
            row += 2;
            printer.PrintFromPosition("Head:  ", row, column);
            row++;
            printer.PrintFromPosition("Torso: ", row, column);
            row++;
            printer.PrintFromPosition("Hands: ", row, column);
            row++;
            printer.PrintFromPosition("Feet:  ", row, column);
            row += 2;
            printer.PrintFromPosition("2 Keys", row, column);
        }

        private void PrintWithinBounds(string msg, Bounds bounds)
        {
            int counter = 0;
            for (int y = bounds.StartXY.Y; y < bounds.EndXY.Y; y++)
            {
                for (int x = bounds.StartXY.X; x < bounds.EndXY.X; x++)
                {
                    if (counter == msg.Length)
                        return;

                    printer.PrintAtPosition(msg[counter], y, x);
                    counter++;
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

        public void ClearSection(Section section)
        {
            foreach (Pixel pixel in pixelsInSections[section.GetType().Name])
            {
                foreach (var position in pixelGrid.GetPlanePositions(pixel.gridPosition))
                {
                    printer.PrintAtPosition(' ', position.Y, position.X);
                }
            }
        }

        #region INITIALIZATION
        private Bounds FindConsoleBoundsOfSection(string sectionName)
        {
            return new Bounds(pixelGrid.GetPlanePosition(pixelsInSections[sectionName].First().gridPosition),
                pixelGrid.GetPlanePosition(pixelsInSections[sectionName].Last().gridPosition));
        }

        private void DefineGridSections(Bounds area, Section section)
        {
            if (!pixelsInSections.ContainsKey(section.GetType().Name))
                pixelsInSections.Add(section.GetType().Name, new HashSet<Pixel>());

            for (int x = area.StartXY.X; x < area.EndXY.X; x++)
            {
                for (int y = area.StartXY.Y; y < area.EndXY.Y; y++)
                {
                    if (pixelGrid.TryGetGridObject(x,y, out Pixel pixel))
                    {
                        pixel.PartOfHUD = section;
                        pixelsInSections[section.GetType().Name].Add(pixel);
                    }
                }
            }
        }

        private void FindBorderPixels()
        {
            for (int x = 0; x < pixelGrid.gridWidth; x++)
            {
                for (int y = 0; y < pixelGrid.gridHeight; y++)
                {
                    if (pixelGrid.TryGetGridObject(x, y, out Pixel pixel) && pixel.PartOfHUD is HUDBorder)
                        borderCells.Add(pixel);
                }
            }
        }
        #endregion
    }
}
