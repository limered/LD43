using System.Linq;
using SystemBase;
using Systems.Combat.Events;
using Systems.Physics;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Math;

namespace Systems.Combat
{
    [GameSystem(typeof(ProjectilePhysicSystem))]
    public class ProjectileSystem : GameSystem<ShooterComponent, ProjectileComponent>
    {
        public override void Register(ShooterComponent component)
        {
            component.ShootCommand
                .Subscribe(vector2 => SpawnProjectile(component, vector2))
                .AddTo(component);
        }

        private static void SpawnProjectile(ShooterComponent component, Vector2 direction)
        {
            var projectile = Object.Instantiate(component.ProjectilePrefab, component.transform.position,
                Quaternion.identity);
            var comp = projectile.GetComponent<ProjectileComponent>();
            comp.Direction = direction;
            comp.StartPosition = component.transform.position;
        }

        public override void Register(ProjectileComponent component)
        {
            component.UpdateAsObservable()
                .Subscribe(unit => UpdateProjectile(component))
                .AddTo(component);

            component.GetComponent<ProjectilePhysicComponent>()
                .CollisionDetected
                .Subscribe(ds => OnCollisionDetected(component, ds))
                .AddTo(component);
        }

        private static void OnCollisionDetected(ProjectileComponent component, RaycastHit2D[] ds)
        {
            if (!ds.Any()) return;

            foreach (var raycastHit2D in ds)
            {
                MessageBroker.Default.Publish(new ProjectileMsgHit
                {
                    HitData = raycastHit2D,
                    Projectile = component
                });
            }
            Object.Destroy(component.gameObject);
        }

        private static void UpdateProjectile(ProjectileComponent component)
        {
            var physics = component.GetComponent<ProjectilePhysicComponent>();

            UpdateVelocity(component, physics);
            UpdateGravity(component, physics);
        }

        private static void UpdateGravity(ProjectileComponent component, ProjectilePhysicComponent physics)
        {
            physics.GravityModifier.Value = (component.transform.position.XY() - component.StartPosition)
                                            .magnitude < component.MaxTravelDistance
                ? 0
                : 1;
        }

        private static void UpdateVelocity(ProjectileComponent component, ProjectilePhysicComponent physics)
        {
            physics.TargetVelocity.Value = component.Direction * component.Speed;
        }
    }
}