using Skalm.Structs;


namespace Skalm.Maps
{
    internal class RandomMap : IMap
    {
        public Dictionary<EMapObjects, (int, List<Vector2Int>)> ObjectsInMap { get; private set; }
        public HashSet<Vector2Int> FloorTiles { get; }
        public HashSet<Vector2Int> DoorTiles { get; }
        public Vector2Int PlayerSpawnPosition { get; set; }
        public Vector2Int GoalPosition { get; set; }

        public RandomMap(HashSet<Vector2Int> floors, HashSet<Vector2Int> doors, int enemies, int items, int keys, int potions)
        {
            FloorTiles = floors;
            DoorTiles = doors;
            ObjectsInMap = new Dictionary<EMapObjects, (int, List<Vector2Int>)>
            {
                { EMapObjects.Enemies, (enemies,new List<Vector2Int>()) },
                { EMapObjects.Items, (items,new List<Vector2Int>()) },
                { EMapObjects.Keys, (keys,new List<Vector2Int>()) },
                { EMapObjects.Potions, (potions,new List<Vector2Int>()) }
            };
        }



    }
}
