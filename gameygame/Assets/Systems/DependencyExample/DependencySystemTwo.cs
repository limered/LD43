using SystemBase;
using Systems.Example;

namespace Systems.DependencyExample
{
    [GameSystem(typeof(DependencySystemMaster), typeof(DependencySystemThree))]
    public class DependencySystemTwo : GameSystem<FunnyMovementComponent>
    {
        public override void Register(FunnyMovementComponent component)
        {
        }
    }
}
