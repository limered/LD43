using SystemBase;
using Systems.Physics;
using Systems.Player;
using UniRx;
using UnityEngine;

namespace Systems.Animation
{
    public class PresentAnimationComponent : GameComponent
    {
        public FloatReactiveProperty AnimationSpeed = new FloatReactiveProperty(0.5f);
    }
}