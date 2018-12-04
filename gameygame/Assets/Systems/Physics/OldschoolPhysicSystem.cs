using System.Linq;
using SystemBase;
using Systems.Physics;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.GameState.Physics
{
    [GameSystem]
    public class OldschoolPhysicSystem : GameSystem<OldschoolPhysicComponent>
    {
        private const float MinMoveDistance = 0.001f;
        private const float CollisionShell = 0.01f;
        private const float MinGroundNormalY = 0.65f;
        
        public override void Register(OldschoolPhysicComponent component)
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

        private static void FixedUpdate(OldschoolPhysicComponent component)
        {
            var rb2D = component.GetComponent<Rigidbody2D>();

            component.Velocity.Value += component.GravityModifier.Value * Physics2D.gravity * Time.fixedDeltaTime;
            component.Velocity.Value = new Vector2(component.TargetVellocity.Value.x, component.Velocity.Value.y);

            component.IsGrounded.Value = false;

            var deltaPosition = component.Velocity.Value * Time.fixedDeltaTime;
            var moveAlongGround = new Vector2(component.GroundNormal.Value.y, -component.GroundNormal.Value.x);

            Move(moveAlongGround * deltaPosition.x, component, rb2D, false);
            Move(Vector2.up * deltaPosition.y, component, rb2D, true);
        }

        private static void Move(Vector2 movement, OldschoolPhysicComponent component, Rigidbody2D rb2D, bool isVertical)
        {
            var distance = movement.magnitude;
            if (distance < MinMoveDistance) return;

            var hitBuffer = new RaycastHit2D[16];
            var collisionCount = rb2D.Cast(movement, component.ContactFilter, hitBuffer, distance + CollisionShell);
            var hitBufferList = hitBuffer.Take(collisionCount).ToArray();

            if (hitBufferList.Any())
            {
                component.CollisionDetected.Execute(hitBufferList);
            }

            foreach (var hit in hitBufferList)
            {
                var currentNormal = hit.normal;
                if (currentNormal.y > MinGroundNormalY)
                {
                    component.IsGrounded.Value = true;
                    if (isVertical)
                    {
                        component.GroundNormal.Value = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                var projection = Vector2.Dot(component.Velocity.Value, currentNormal);
                if (projection < 0)
                {
                    component.Velocity.Value = component.Velocity.Value - projection * currentNormal;
                }

                var modifiedDistance = hit.distance - CollisionShell;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

            rb2D.position = rb2D.position + movement.normalized * distance;
        }
    }
}
