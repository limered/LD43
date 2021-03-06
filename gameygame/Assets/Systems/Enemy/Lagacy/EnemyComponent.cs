﻿using SystemBase;
using UnityEngine;

namespace Systems.Enemy.Lagacy
{
    public class EnemyComponent : GameComponent
    {
        public float MovementMaxSpeed;
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
