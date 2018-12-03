using SystemBase;
using Systems.Health;
using UnityEngine;
using Utils.Enums;

namespace Systems.Enemy.Patrol
{
    
    public class PatrolEnemySpawnerComponent : GameComponent
    {
        public GameObject PatrolPrefab;
        public GameObject LeftWaypoint;
        public GameObject RightWaypoint;
        public UsefulEnums.HorizontalDirection StartDirection;
        public float MovementMaxSpeed = 25;
    }
}
