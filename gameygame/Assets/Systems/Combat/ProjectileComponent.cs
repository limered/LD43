using SystemBase;
using Systems.Physics;
using UnityEngine;

namespace Systems.Combat
{
    [RequireComponent(typeof(ProjectilePhysicComponent), typeof(ShooterComponent))]
    public class ProjectileComponent : GameComponent
    {
        public Vector2 StartPosition;
        public Vector2 Direction;
        public float Speed = 5;
        public float MaxTravelDistance;
    }
}