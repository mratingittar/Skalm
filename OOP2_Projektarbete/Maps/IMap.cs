using Skalm.Structs;


namespace Skalm.Maps
{
    internal interface IMap
    {
        Dictionary<EMapObjects, (int, List<Vector2Int>)> ObjectsInMap { get; }
        HashSet<Vector2Int> FloorTiles { get; }
        HashSet<Vector2Int> DoorTiles { get; }
        Vector2Int PlayerSpawnPosition { get; set; }
        Vector2Int GoalPosition { get; set; }
    }
}
