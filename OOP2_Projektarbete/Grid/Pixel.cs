using Skalm.Structs;

namespace Skalm.Grid
{
    internal class Pixel
    {
        public readonly Vector2Int gridPosition;
        public IHUD PartOfHUD { get; set; }

        public Pixel(Vector2Int gridPosition, IHUD partOfHUD)
        {
            this.gridPosition = gridPosition;
            PartOfHUD = partOfHUD;
        }
    }
}
