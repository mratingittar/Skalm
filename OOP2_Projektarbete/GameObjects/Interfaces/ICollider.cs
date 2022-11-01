
namespace Skalm.GameObjects.Interfaces
{
    internal interface ICollider
    {
        bool ColliderIsActive { get; }
        void OnCollision();
    }
}
