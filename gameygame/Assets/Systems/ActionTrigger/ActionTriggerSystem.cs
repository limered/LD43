using System;
using SystemBase;

namespace Systems.ActionTrigger
{
    [GameSystem]
    public class ActionTriggerSystem : GameSystem<TriggerComponent>
    {
        public override void Register(TriggerComponent component)
        {
            throw new NotImplementedException();
        }
    }

    public class TriggerComponent : GameComponent
    {
        public Action ActionToTrigger;
    }
}
