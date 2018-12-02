﻿using SystemBase;

namespace Systems.Enemy
{
    public class EnemyComponent : GameComponent
    {
        public float MovementMaxSpeed = 4;
        public bool IsActive;
        public float ShootingDelay = .5f;
    }
}