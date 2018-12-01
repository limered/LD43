using System;
using SystemBase;
using UniRx;

namespace Systems.GameState.Physics
{
    public class PhysicalEntityComponent : GameComponent
    {
        public BoolReactiveProperty Gravity = new BoolReactiveProperty(true);
        public ReactiveCollection<Type> CollidesWith = new ReactiveCollection<Type>();
    }
}