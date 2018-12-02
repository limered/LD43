﻿using SystemBase;
using UnityEngine;

namespace Systems.Enemy
{
    public class PatrolEnemySpawnerComponent : GameComponent
    {
        public GameObject PatrolPrefab;
        public GameObject LeftWaypoint;
        public GameObject RightWaypoint;
        public EnemyDirection StartDirection;
        public float MovementMaxSpeed = 25;
    }
}
