using SystemBase;
using UnityEngine;

namespace Systems.Enemy
{
    public class EnemyComponent : GameComponent
    {
        public float MovementMaxSpeed { get; set; }
        public bool IsActive { get; set; }
        public float ShootingDelay { get; set; }

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
