using System;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Physics
{
    public class ProjectilePhysicComponent : GameComponent
    {
        public FloatReactiveProperty GravityModifier = new FloatReactiveProperty();

        [NonSerialized]
        public Vector2ReactiveProperty Velocity = new Vector2ReactiveProperty(Vector2.zero);
        [NonSerialized]
        public Vector2ReactiveProperty TargetVelocity = new Vector2ReactiveProperty(Vector2.zero);
        [NonSerialized]
        public ReactiveCommand<RaycastHit2D[]> CollisionDetected = new ReactiveCommand<RaycastHit2D[]>();

        public ContactFilter2D ContactFilter { get; set; }
    }
}