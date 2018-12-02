using System.Linq;
using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Physics
{
    [GameSystem]
    public class ProjectilePhysicSystem : GameSystem<ProjectilePhysicComponent>
    {
        private const float CollisionShell = 0.001f;

        public override void Register(ProjectilePhysicComponent component)
        {
            component.FixedUpdateAsObservable()
                .Subscribe(_ => AnimateProjectile(component))
                .AddTo(component);

            var contactFilter = new ContactFilter2D
            {
                useTriggers = false,
                useLayerMask = true
            };
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(component.gameObject.layer));
            component.ContactFilter = contactFilter;
        }

        private static void AnimateProjectile(ProjectilePhysicComponent component)
        {
            var rb2D = component.GetComponent<Rigidbody2D>();

            component.Velocity.Value += component.GravityModifier.Value * Physics2D.gravity * Time.fixedDeltaTime;
            component.Velocity.Value = new Vector2(component.TargetVelocity.Value.x, component.Velocity.Value.y);
            var deltaPosition = component.Velocity.Value * Time.fixedDeltaTime;

            rb2D.position = rb2D.position + deltaPosition;

            var hitBuffer = new RaycastHit2D[16];
            var collisionCount = rb2D.Cast(deltaPosition.normalized, component.ContactFilter, hitBuffer, CollisionShell);
            var hitBufferList = hitBuffer.Take(collisionCount).ToArray();

            component.CollisionDetected.Execute(hitBufferList);
        }
    }
}