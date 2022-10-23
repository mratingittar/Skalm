using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.Grid
{
    internal class PixelController
    {
        public int InventoryIndex { get; set; }
        public int InventoryRowsAvailable { get; private set; }

        private int _inventoryStartRow;
        private HashSet<Pixel> _borderCells;
        private Dictionary<string, HashSet<Pixel>> _pixelsInSections;
        private readonly Grid2D<Pixel> _pixelGrid;
        private readonly IPrinter _printer;
        private readonly IEraser _eraser;
        private readonly Bounds _messageConsole;
        private readonly Bounds _mainStatsConsole;
        private readonly Bounds _subStatsConsole;

        // CONSTRUCTOR I
        public PixelController(Grid2D<Pixel> pixelGrid, Dictionary<string, Bounds> sectionBounds, IPrinter printer, IEraser eraser)
        {
            this._pixelGrid = pixelGrid;
            this._printer = printer;
            this._eraser = eraser;
            _pixelsInSections = new Dictionary<string, HashSet<Pixel>>();
            _borderCells = new HashSet<Pixel>();
            InventoryIndex = -1;

            Player.OnPlayerStatsUpdated += DisplayStats;
            Player.OnPlayerInventoryUpdated += DisplayInventory;

            DefineGridSections(sectionBounds["mapBounds"], new MapSection());
            DefineGridSections(sectionBounds["messageBounds"], new MessageSection());
            DefineGridSections(sectionBounds["mainStatsBounds"], new MainStatsSection());
            DefineGridSections(sectionBounds["subStatsBounds"], new SubStatsSection());

            FindBorderPixels();

            Bounds message = FindConsoleBoundsOfSection("MessageSection");
            _messageConsole = new Bounds(new Vector2Int(message.StartXY.X + 2, message.StartXY.Y + 1), new Vector2Int(message.EndXY.X - 2, message.EndXY.Y - 1));

            Bounds mainStats = FindConsoleBoundsOfSection("MainStatsSection");
            _mainStatsConsole = new Bounds(new Vector2Int(mainStats.StartXY.X + 2, mainStats.StartXY.Y + 1), new Vector2Int(mainStats.EndXY.X - 2, mainStats.EndXY.Y - 1));

            Bounds subStats = FindConsoleBoundsOfSection("SubStatsSection");
            _subStatsConsole = new Bounds(new Vector2Int(subStats.StartXY.X + 2, subStats.StartXY.Y + 1), new Vector2Int(subStats.EndXY.X - 2, subStats.EndXY.Y - 1));
        }



        // DISPLAY MESSAGE BOTTOM SECTION
        public void DisplayMessage(string msg, bool showMarker = false)
        {
            ClearSection("MessageSection");
            PrintWithinBounds(msg, _messageConsole);
            if (showMarker)
                Console.CursorVisible = true;
        }

        // DISPLAY STATS MAIN SECTION
        public void DisplayStats(ActorStatsObject playerStats, int currentFloor)
        {
            ClearSection("MainStatsSection");

            string floor = $"Floor {currentFloor}";

            string name = playerStats.name;
            if (name.Length > _mainStatsConsole.Size.Width)
                name = name.Remove(_mainStatsConsole.Size.Width);

            StatsObject statsObject = playerStats.stats;

            int column = _mainStatsConsole.StartXY.X;
            int row = _mainStatsConsole.StartXY.Y;

            _printer.PrintFromPosition(name, row, column);
            row++;
            _printer.PrintFromPosition(TextTools.RepeatChar('─', name.Length), row, column);
            row++;
            _printer.PrintFromPosition($"Level {playerStats.Level}", row, column);
            row++;
            _printer.PrintFromPosition($"Experience:  {playerStats.XP}", row, column);
            row++;
            _printer.PrintFromPosition($"Hit points:  {playerStats.GetCurrentHP()} / {statsObject.statsArr[(int)EStats.HP].GetValue()}", row, column);
            row++;
            _printer.PrintFromPosition($"Armor: {statsObject.statsArr[(int)EStats.Armor].GetValue()}", row, column);
            row++;
            _printer.PrintFromPosition($"Base damage: {statsObject.statsArr[(int)EStats.BaseDamage].GetValue()}", row, column);
            row += 2;
            _printer.PrintFromPosition($"Str {statsObject.statsArr[(int)EStats.Strength].GetValue()} " +
                $"| Dex {statsObject.statsArr[(int)EStats.Dexterity].GetValue()} " +
                $"| Con {statsObject.statsArr[(int)EStats.Constitution].GetValue()} " +
                $"| Int {statsObject.statsArr[(int)EStats.Intelligence].GetValue()} " +
                $"| Lck {statsObject.statsArr[(int)EStats.Luck].GetValue()}", row, column);

            _printer.PrintFromPosition(floor, _mainStatsConsole.StartXY.Y, _mainStatsConsole.EndXY.X - floor.Length);
        }

        // DISPLAY INVENTORY SECTION
        public void DisplayInventory(EquipmentManager im)
        {
            ClearSection("SubStatsSection");

            int column = _subStatsConsole.StartXY.X;
            int row = _subStatsConsole.StartXY.Y;

            _printer.PrintFromPosition("Equipment", row, column);
            row++;
            _printer.PrintFromPosition("─────────", row, column);
            row++;
            _printer.PrintFromPosition($"Head:       {im.equipArr[(int)EEqSlots.Head].Name}", row, column);
            row++;
            _printer.PrintFromPosition($"Torso:      {im.equipArr[(int)EEqSlots.Torso].Name}", row, column);
            row++;
            _printer.PrintFromPosition($"Left hand:  {im.equipArr[(int)EEqSlots.LHand].Name}", row, column);
            row++;
            _printer.PrintFromPosition($"Right hand: {im.equipArr[(int)EEqSlots.RHand].Name}", row, column);
            row++;
            _printer.PrintFromPosition($"Legs:       {im.equipArr[(int)EEqSlots.Legs].Name}", row, column);
            row++;
            _printer.PrintFromPosition($"Feet:       {im.equipArr[(int)EEqSlots.Feet].Name}", row, column);
            row++;
            _printer.PrintFromPosition($"Finger:     {im.equipArr[(int)EEqSlots.Finger].Name}", row, column);
            row += 4;

            _printer.PrintFromPosition("Inventory", row, column);
            row++;
            _printer.PrintFromPosition("─────────", row, column);
            row++;

            _inventoryStartRow = row;
            InventoryRowsAvailable = _subStatsConsole.EndXY.Y - _inventoryStartRow;

            PrintInventory(im.inventory.itemList, row, column, InventoryIndex);
        }

        private void PrintInventory(List<Item> items, int row, int column, int selectionIndex)
        {
            int inventoryPage = Math.Max((selectionIndex + InventoryRowsAvailable) / InventoryRowsAvailable, 1) - 1;
            int offset = selectionIndex >= InventoryRowsAvailable ? InventoryRowsAvailable * inventoryPage : 0;

            for (int i = 0; i < Math.Min(InventoryRowsAvailable, items.Count() - offset); i++)
            {
                if (i + offset == selectionIndex)
                    _printer.PrintFromPosition(items[i + offset].Name, row, column, true);
                else
                    _printer.PrintFromPosition(items[i + offset].Name, row, column);
                row++;
            }

            if (inventoryPage == 0 && items.Count() > InventoryRowsAvailable)
                _printer.PrintFromPosition("  ... ►", row, column);
            else if (inventoryPage > 0 && items.Count() > InventoryRowsAvailable * (inventoryPage + 1))
                _printer.PrintFromPosition("◄ ... ►", row, column);
            else if (inventoryPage > 0 && items.Count() < InventoryRowsAvailable * (inventoryPage + 1))
            {
                _printer.PrintFromPosition("◄ ... ", row, column);
                _eraser.EraseArea(new Bounds(new Vector2Int(column, row + 1), _subStatsConsole.EndXY));
            }
            else
                _eraser.EraseArea(new Bounds(new Vector2Int(column, row), _subStatsConsole.EndXY));
        }

        // PRINT WITHIN BOUNDS
        private void PrintWithinBounds(string msg, Bounds bounds)
        {
            int counter = 0;
            for (int y = bounds.StartXY.Y; y < bounds.EndXY.Y; y++)
            {
                for (int x = bounds.StartXY.X; x < bounds.EndXY.X; x++)
                {
                    if (counter == msg.Length)
                        return;

                    _printer.PrintAtPosition(msg[counter], y, x);
                    counter++;
                }
            }
        }

        // PRINT BORDERS
        public void PrintBorders()
        {
            foreach (Pixel cell in _borderCells)
            {
                foreach (var position in _pixelGrid.GetPlanePositions(cell.gridPosition.X, cell.gridPosition.Y))
                {
                    _printer.PrintAtPosition((cell.PartOfHUD as HUDBorder)!.borderCharacter, position.Y, position.X);
                }
            }
        }

        // CLEAR SECTION
        public void ClearSection(string section)
        {
            foreach (Pixel pixel in _pixelsInSections[section])
            {
                foreach (var position in _pixelGrid.GetPlanePositions(pixel.gridPosition))
                {
                    _printer.PrintAtPosition(' ', position.Y, position.X);
                }
            }
        }

        public Vector2Int GetMapOrigin()
        {
            return _pixelGrid.GetPlanePosition(_pixelsInSections["MapSection"].First().gridPosition);
        }

        #region INITIALIZATION
        // FIND CONSOLE BOUNDS OF SECTION
        private Bounds FindConsoleBoundsOfSection(string sectionName)
        {
            return new Bounds(_pixelGrid.GetPlanePosition(_pixelsInSections[sectionName].First().gridPosition),
                _pixelGrid.GetPlanePosition(_pixelsInSections[sectionName].Last().gridPosition));
        }

        // DEFINE GRID SECTIONS
        private void DefineGridSections(Bounds area, Section section)
        {
            if (!_pixelsInSections.ContainsKey(section.GetType().Name))
                _pixelsInSections.Add(section.GetType().Name, new HashSet<Pixel>());

            for (int x = area.StartXY.X; x < area.EndXY.X; x++)
            {
                for (int y = area.StartXY.Y; y < area.EndXY.Y; y++)
                {
                    if (_pixelGrid.TryGetGridObject(x, y, out Pixel pixel))
                    {
                        pixel.PartOfHUD = section;
                        _pixelsInSections[section.GetType().Name].Add(pixel);
                    }
                }
            }
        }

        // FIND BORDER PIXELS
        private void FindBorderPixels()
        {
            for (int x = 0; x < _pixelGrid.gridWidth; x++)
            {
                for (int y = 0; y < _pixelGrid.gridHeight; y++)
                {
                    if (_pixelGrid.TryGetGridObject(x, y, out Pixel pixel) && pixel.PartOfHUD is HUDBorder)
                        _borderCells.Add(pixel);
                }
            }
        }
        #endregion
    }
}
