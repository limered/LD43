using SystemBase;
using Systems.Player;

namespace Systems.Camera
{
    [GameSystem(typeof(PlayerSystem))]
    public class CameraSystem : GameSystem<PlayerComponent, CameraComponent>
    {


        public override void Register(PlayerComponent component)
        {
            throw new System.NotImplementedException();
        }

        public override void Register(CameraComponent component)
        {
            throw new System.NotImplementedException();
        }
    }

    public class CameraComponent : GameComponent
    {
    }
}
