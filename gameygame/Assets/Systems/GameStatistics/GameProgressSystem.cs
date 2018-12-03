using SystemBase;

namespace Systems.GameStatistics
{
    [GameSystem]
    public class GameProgressSystem : GameSystem<CollectibleComponent>
    {
        public override void Register(CollectibleComponent component)
        {
            throw new System.NotImplementedException();
        }
    }

    public class CollectibleComponent : GameComponent
    {
        public string UniqueId;
    }
}
