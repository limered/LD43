using SystemBase;
using Systems.Example;

namespace Systems.DependencyExample
{
    [GameSystem]
    public class DependencySystemMaster : GameSystem<FunnyMovementComponent>
    {
        public override void Register(FunnyMovementComponent component)
        {
        }
    }
}
