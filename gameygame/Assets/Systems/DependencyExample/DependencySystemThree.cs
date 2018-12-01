using SystemBase;
using Systems.Example;

namespace Systems.DependencyExample
{
    [GameSystem(typeof(DependencySystemOne), typeof(FunnyMovementSystem))]
    public class DependencySystemThree : GameSystem<FunnyMovementComponent>
    {
        public override void Register(FunnyMovementComponent component)
        {
        }
    }
}
