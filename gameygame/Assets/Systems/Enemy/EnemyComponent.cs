using SystemBase;
using UnityEngine;

namespace Systems.Enemy
{
    public class EnemyComponent : GameComponent
    {
        public float MovementMaxSpeed = 4;
        public bool IsActive;
        public float ShootingDelay = .5f;

        private Animator _animator;
        public Animator Animator {
            get{
                if(_animator == null) {
                    _animator = GetComponentInChildren<Animator>();
                }
                return _animator;
            }
        }
    }
}
