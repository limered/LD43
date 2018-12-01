using System;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.GameState.Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class OldschoolPhysicComponent : GameComponent
    {
        public ReactiveCollection<Type> CollidesWith = new ReactiveCollection<Type>();
        public FloatReactiveProperty GravityModifier = new FloatReactiveProperty(1);
        public BoolReactiveProperty IsGrounded = new BoolReactiveProperty(false);
        
        [NonSerialized]
        public Vector2ReactiveProperty Velocity = new Vector2ReactiveProperty(Vector2.zero);
        [NonSerialized]
        public Vector2ReactiveProperty GroundNormal = new Vector2ReactiveProperty();
        [NonSerialized]
        public Vector2ReactiveProperty TargetVellocity = new Vector2ReactiveProperty();

        
        public ContactFilter2D ContactFilter { get; set; }
    }
}