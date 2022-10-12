using Skalm.Structs;

namespace Skalm.Grid
{
    internal class Pixel
    {
        public readonly Vector2Int gridPosition;
        public readonly List<Vector2Int> planePositions;
        public IHUD PartOfHUD { get; set; }

        public Pixel(Vector2Int gridPosition, List<Vector2Int> planePositions, IHUD partOfHUD)
        {
            this.gridPosition = gridPosition;
            this.planePositions = planePositions;
            PartOfHUD = partOfHUD;
        }
    }
}
