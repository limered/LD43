using SystemBase;
using Systems.Combat;
using Systems.GameState.Physics;
using Systems.Physics;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Player
{
    [GameSystem(typeof(PlayerSystem), typeof(OldschoolPhysicSystem), typeof(ProjectileSystem))]
    public class PlayerControlSystem : GameSystem<PlayerComponent>
    {
        public override void Register(PlayerComponent component)
        {
            component
                .UpdateAsObservable()
                .Subscribe(_=>ComputeVelocity(component))
                .AddTo(component);

            component
                .UpdateAsObservable()
                .Subscribe(_=> Shoot(component))
                .AddTo(component);
        }

        private static void Shoot(PlayerComponent component)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                var shootDirection = component.Direction == PlayerDirection.Left ? Vector2.left : Vector2.right;
                component.GetComponent<ShooterComponent>()
                    .ShootCommand
                    .Execute(shootDirection);
            }
        }

        private static void ComputeVelocity(PlayerComponent component)
        {
            var physics = component.GetComponent<OldschoolPhysicComponent>();
            var move = Vector2.zero;

            move.x = Input.GetAxis("Horizontal") * component.MovementMaxSpeed;
            component.Direction = Input.GetAxis("Horizontal") < 0 ? 
                PlayerDirection.Left : 
                Input.GetAxis("Horizontal") > 0 ? 
                    PlayerDirection.Right : 
                    component.Direction;

            if (Input.GetButtonDown("Jump") && physics.IsGrounded.Value)
            {
                physics.Velocity.Value = new Vector2(physics.Velocity.Value.x, component.JumpTakeofSpeed);
            }
            else if(Input.GetButtonUp("Jump") && physics.Velocity.Value.y > 0)
            {
                physics.Velocity.Value = new Vector2(physics.Velocity.Value.x, physics.Velocity.Value.y * 0.5f);
            }

            physics.TargetVellocity.Value = move;
        }
    }

    public enum PlayerDirection
    {
        Left = 1,
        Right = 2
    }
}
