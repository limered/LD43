using SystemBase;
using Systems.Example;

namespace Systems.DependencyExample
{
    [GameSystem(typeof(DependencySystemMaster))]
    public class DependencySystemOne : GameSystem<FunnyMovementComponent>
    {
        public override void Register(FunnyMovementComponent component)
        {
        }
    }
}
