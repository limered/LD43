using SystemBase;
using Systems.Physics;
using UnityEngine;
using Utils.Enums;

namespace Systems.Enemy.Components
{
    [RequireComponent(typeof(OldschoolPhysicComponent))]
    public class PatrolMovementComponent : GameComponent
    {
        public UsefulEnums.HorizontalDirection MovementDirection;
        public GameObject LeftWaypoint;
        public GameObject RightWaypoint;
        public float MovementMaxSpeed;
        private OldschoolPhysicComponent _physics;

        public OldschoolPhysicComponent Physics
        {
            get
            {
                if (!_physics)
                {
                    _physics = GetComponent<OldschoolPhysicComponent>();
                }
                return _physics;
            }
        }
    }
}