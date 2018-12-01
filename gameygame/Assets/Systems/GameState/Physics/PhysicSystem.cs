using System.Linq;
using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.GameState.Physics
{
    [GameSystem]
    public class PhysicSystem : GameSystem<PhysicalEntityComponent>
    {
        private const float MinMoveDistance = 0.001f;
        private const float CollisionShell = 0.01f;
        private const float MinGroundNormalY = 0.65f;
        

        public override void Init()
        {
            base.Init();
        }

        public override void Register(PhysicalEntityComponent component)
        {
            component
                .FixedUpdateAsObservable()
                .Subscribe(_=>FixedUpdate(component))
                .AddTo(component);

            var contactFilter = new ContactFilter2D
            {
                useTriggers = false,
                useLayerMask = true
            };
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(component.gameObject.layer));
            component.ContactFilter = contactFilter;
        }

        private static void FixedUpdate(PhysicalEntityComponent component)
        {
            component.IsGrounded.Value = false;

            var rb2D = component.GetComponent<Rigidbody2D>();

            component.Velocity.Value += component.GravityModifier.Value * Physics2D.gravity * Time.deltaTime;
            var deltaPosition = component.Velocity.Value * Time.deltaTime;

            var move = Vector2.up * deltaPosition;

            var distance = move.magnitude > MinMoveDistance ? 
                ComputeDistanceToGround(component, move, rb2D) : 
                move.magnitude;

            rb2D.position = rb2D.position + move.normalized * distance;
        }

        private static float ComputeDistanceToGround(PhysicalEntityComponent component, Vector2 move, Rigidbody2D rb2D)
        {
            var distance = move.magnitude;

            var hitBuffer = new RaycastHit2D[16];
            var collisionCount = rb2D.Cast(move, component.ContactFilter, hitBuffer, distance + CollisionShell);
            var hitBufferList = hitBuffer.Take(collisionCount);

            foreach (var hit in hitBufferList)
            {
                var currentNormal = hit.normal;
                if (currentNormal.y > MinGroundNormalY)
                {
                    component.IsGrounded.Value = true;
                    component.GroundNormal.Value = currentNormal;
                    currentNormal.x = 0;
                }

                var projection = Vector2.Dot(component.Velocity.Value, currentNormal);
                if (projection < 0)
                {
                    component.Velocity.Value = component.Velocity.Value - projection * currentNormal;
                }

                var modifiedDistance = hit.distance - CollisionShell;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

            return distance;
        }
    }
}
