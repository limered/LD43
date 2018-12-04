using SystemBase;
using Systems.Physics;
using Systems.Player;
using UniRx;
using UnityEngine;

namespace Systems.Animation
{
    public class RotationAnimationComponent : GameComponent
    {
        public bool StartWithRandomRotation = false;
        public float ConstantRotation = 0;
    }
}