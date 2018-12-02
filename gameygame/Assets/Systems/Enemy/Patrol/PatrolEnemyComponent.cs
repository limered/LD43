using Systems.Health;
using Systems.Physics;
using UnityEngine;

namespace Systems.Enemy.Patrol
{
    [RequireComponent(typeof(HealthComponent))]
    public class PatrolEnemyComponent : EnemyComponent
    {
        public EnemyDirection Direction;
        public GameObject LeftWaypoint;
        public GameObject RightWaypoint;

        private OldschoolPhysicComponent _physics;

        private new void Start()
        {
            base.Start();

            _physics = GetComponent<OldschoolPhysicComponent>();
            UpdateTargetVelocity();
        }

        private void Update()
        {
            if (LeftWaypoint && transform.position.x <= LeftWaypoint.transform.position.x)
            {
                Direction = EnemyDirection.Right;
                UpdateTargetVelocity();
            }
            else if (RightWaypoint && transform.position.x >= RightWaypoint.transform.position.x)
            {
                Direction = EnemyDirection.Left;
                UpdateTargetVelocity();
            }
        }

        private void UpdateTargetVelocity()
        {
            var velocity = (int) Direction * MovementMaxSpeed * Time.deltaTime;
            _physics.TargetVellocity.Value = new Vector2(velocity, 0);
        }
    }
}
