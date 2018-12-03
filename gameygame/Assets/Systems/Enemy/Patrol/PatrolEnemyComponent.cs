using Systems.Health;
using Systems.Physics;
using UnityEngine;
using Utils.Enums;

namespace Systems.Enemy.Patrol
{
    [RequireComponent(typeof(HealthComponent))]
    public class PatrolEnemyComponent : EnemyComponent
    {
        public UsefulEnums.HorizontalDirection Direction;
        public GameObject LeftWaypoint;
        public GameObject RightWaypoint;

        private OldschoolPhysicComponent _physics;

        private new void Start()
        {
            base.Start();

            _physics = GetComponent<OldschoolPhysicComponent>();
        }

        private void Update()
        {
            if (LeftWaypoint && transform.position.x <= LeftWaypoint.transform.position.x)
            {
                Direction = UsefulEnums.HorizontalDirection.Right;
            }
            else if (RightWaypoint && transform.position.x >= RightWaypoint.transform.position.x)
            {
                Direction = UsefulEnums.HorizontalDirection.Left;
            }
            UpdateTargetVelocity();
        }

        private void UpdateTargetVelocity()
        {
            var velocity = (int) Direction * MovementMaxSpeed;
            _physics.TargetVellocity.Value = new Vector2(velocity, 0);
        }
    }
}
