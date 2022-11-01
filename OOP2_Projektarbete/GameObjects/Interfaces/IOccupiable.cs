

namespace Skalm.GameObjects.Interfaces
{
    internal interface IOccupiable
    {
        Stack<GameObject> ObjectsOnTile { get; }
        bool ActorPresent { get; set; }
    }
}
