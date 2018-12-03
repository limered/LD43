using SystemBase;
using Systems.Physics;
using UniRx;
using UnityEngine;
using Utils.Enums;

namespace Systems.Player
{
    [RequireComponent(typeof(OldschoolPhysicComponent))]
    public class PlayerComponent : GameComponent
    {
        public float TimBetweenHits = 0.1f;
        public float JumpTakeofSpeed = 5;
        public float MovementMaxSpeed = 2;
        public UsefulEnums.HorizontalDirection Direction = UsefulEnums.HorizontalDirection.Right;

        public float HealthLostPerProjectile;

        public float FullSize;
        public float SmallSize;
        public FloatReactiveProperty CurrentSize;

        private Animator _animator;
        public Animator Animator {
            get{
                if(_animator == null) {
                    _animator = GetComponentInChildren<Animator>();
                }
                return _animator;
            }
        }

        public float LastHitTimestamp { get; set; }
    }
}