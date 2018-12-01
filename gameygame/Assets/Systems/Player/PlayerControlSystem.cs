using SystemBase;

namespace Systems.Player
{
    [GameSystem(typeof(PlayerSystem))]
    public class PlayerControlSystem : GameSystem<PlayerComponent>
    {
        public override void Register(PlayerComponent component)
        {
            
        }
    }
}
