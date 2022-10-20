using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
using Skalm.Map.Tile;
using Skalm.States;
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
            ClearSection("MessageSection");
            PrintWithinBounds(msg, messageConsole);
        }

        public void DisplayStats(ActorStatsObject playerStats)
        {
            string name = playerStats.name;
            if (name.Length > mainStatsConsole.Size.Width)
                name = name.Remove(mainStatsConsole.Size.Width);

            StatsObject statsObject = playerStats.stats;

            int column = mainStatsConsole.StartXY.X;
            int row = mainStatsConsole.StartXY.Y;

            printer.PrintFromPosition(name, row, column);
            row++;
            printer.PrintFromPosition(TextTools.RepeatChar('─', name.Length), row, column);
            row++;
            printer.PrintFromPosition($"Lives: {playerStats.GetCurrentHP()} / {statsObject.statsArr[(int)EStats.HP].GetValue()}", row, column);
            row++;
            printer.PrintFromPosition($"Base damage: {statsObject.statsArr[(int)EStats.BaseDamage].GetValue()}", row, column);
            row += 2;
            printer.PrintFromPosition($"Str {statsObject.statsArr[(int)EStats.Strength].GetValue()} " +
                $"| Dex {statsObject.statsArr[(int)EStats.Dexterity].GetValue()} " +
                $"| Con {statsObject.statsArr[(int)EStats.Constitution].GetValue()} " +
                $"| Int {statsObject.statsArr[(int)EStats.Intelligence].GetValue()} " +
                $"| Lck {statsObject.statsArr[(int)EStats.Luck].GetValue()}", row, column);
        }

        public void DisplayInventory(EquipmentManager im)
        {
            int column = subStatsConsole.StartXY.X;
            int row = subStatsConsole.StartXY.Y;



            printer.PrintFromPosition("Equipment", row, column);
            row++;
            printer.PrintFromPosition("─────────", row, column);
            row += 2;
            printer.PrintFromPosition($"Head:       {im.equipArr[(int)EquipSlots.Head].itemName}", row, column);
            row++;
            printer.PrintFromPosition($"Torso:      {im.equipArr[(int)EquipSlots.Torso].itemName}", row, column);
            row++;
            printer.PrintFromPosition($"Left hand:  {im.equipArr[(int)EquipSlots.LHand].itemName}", row, column);
            row++;
            printer.PrintFromPosition($"Left ring:  {im.equipArr[(int)EquipSlots.LFinger].itemName}", row, column);
            row++;
            printer.PrintFromPosition($"Right hand: {im.equipArr[(int)EquipSlots.RHand].itemName}", row, column);
            row++;
            printer.PrintFromPosition($"Right ring: {im.equipArr[(int)EquipSlots.RFinger].itemName}", row, column);
            row++;
            printer.PrintFromPosition($"Feet:       {im.equipArr[(int)EquipSlots.Feet].itemName}", row, column);
            row += 3;
            printer.PrintFromPosition("Inventory", row, column);
            row++;
            printer.PrintFromPosition("─────────", row, column);
            row += 2;
            foreach (var item in im.inventory.itemList)
            {
                printer.PrintFromPosition(item.itemName, row, column);
                row++;
            }
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

        public void ClearSection(string section)
        {
            foreach (Pixel pixel in pixelsInSections[section])
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
