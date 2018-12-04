using SystemBase;

namespace Systems.GameStatistics
{
    [GameSystem]
    public class GameProgressSystem : GameSystem<StatsComponent>
    {
        public override void Register(StatsComponent component)
        {
            
        }
    }

    public class StatsComponent : GameComponent
    {
    }
}
