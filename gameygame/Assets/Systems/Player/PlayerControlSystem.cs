using SystemBase;
using Systems.Combat;
using Systems.Combat.Actions;
using Systems.GameState.Physics;
using Systems.Health;
using Systems.Health.Actions;
using Systems.Physics;
using Assets.Systems.Sound;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Enums;

namespace Systems.Player
{
    [GameSystem(typeof(PlayerSystem), typeof(OldschoolPhysicSystem), typeof(ProjectileSystem))]
    public class PlayerControlSystem : GameSystem<PlayerComponent>
    {
        public override void Register(PlayerComponent component)
        {
            component
                .UpdateAsObservable()
                .Subscribe(_ => ComputeVelocity(component))
                .AddTo(component);

            component
                .UpdateAsObservable()
                .Subscribe(_ => Shoot(component))
                .AddTo(component);
        }

        private static void Shoot(PlayerComponent component)
        {
            if (!Input.GetButtonDown("Fire1")) return;

            if (component.GetComponent<HealthComponent>().CurrentHealth.Value <= 1) return;

            var shootDirection = component.Direction == UsefulEnums.HorizontalDirection.Left ? Vector2.left : Vector2.right;

            "PlayerShoot".Play();

            MessageBroker.Default
                .Publish(new CombatActShoot
                {
                    Direction = shootDirection,
                    Shooter = component.gameObject
                });

            MessageBroker.Default
                .Publish(new HealthActSubtract
                {
                    CanKill = false,
                    Amount = component.HealthLostPerProjectile,
                    Target = component.gameObject
                });
        }

        private static void ComputeVelocity(PlayerComponent component)
        {
            var physics = component.GetComponent<OldschoolPhysicComponent>();
            var move = Vector2.zero;
            var hAxis = Input.GetAxis("Horizontal");

            move.x = hAxis * component.MovementMaxSpeed;
            component.Direction = hAxis < 0
            ? UsefulEnums.HorizontalDirection.Left
            : hAxis > 0
                ? UsefulEnums.HorizontalDirection.Right
                : component.Direction;

            if (Input.GetButtonDown("Jump") && physics.IsGrounded.Value)
            {
                "Jump".Play();
                physics.Velocity.Value = new Vector2(physics.Velocity.Value.x, component.JumpTakeofSpeed);
            }
            else if (Input.GetButtonUp("Jump") && physics.Velocity.Value.y > 0)
            {
                physics.Velocity.Value = new Vector2(physics.Velocity.Value.x, physics.Velocity.Value.y * 0.5f);
            }

            physics.TargetVellocity.Value = move;
        }
    }
}
