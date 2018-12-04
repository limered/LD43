using SystemBase;
using Systems.Enemy.Components;
using Systems.Physics;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Enums;

namespace Systems.Enemy
{
    [GameSystem(typeof(PlayerSystem))]
    public class EnemyMovementAiSystem : GameSystem<PatrolMovementComponent, AimAtPlayerComponent, PlayerComponent, JumpingJackMovementComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>();

        public override void Register(PatrolMovementComponent component)
        {
            component.UpdateAsObservable()
                .Select(_ => component)
                .Subscribe(MovePatrol)
                .AddTo(component);
        }

        public override void Register(AimAtPlayerComponent component)
        {
            _player.Where(playerComponent => playerComponent)
                .Select(playerComponent => new { playerComponent, component })
                .Subscribe(obj =>
                {
                    component.UpdateAsObservable()
                        .Select(_ => obj)
                        .Subscribe(o => { UpdateDirection(o.component, o.playerComponent); })
                        .AddTo(obj.component);
                })
                .AddTo(component);
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        private static void MovePatrol(PatrolMovementComponent comp)
        {
            if (!comp.LeftWaypoint || !comp.RightWaypoint) return;
            if (comp.transform.position.x <= comp.LeftWaypoint.transform.position.x)
            {
                comp.MovementDirection = UsefulEnums.HorizontalDirection.Right;
            }
            else if (comp.transform.position.x >= comp.RightWaypoint.transform.position.x)
            {
                comp.MovementDirection = UsefulEnums.HorizontalDirection.Left;
            }

            var velocity = (int)comp.MovementDirection * comp.MovementMaxSpeed;
            comp.Physics.TargetVellocity.Value = new Vector2(velocity, 0);
        }

        private static void UpdateDirection(AimAtPlayerComponent component, PlayerComponent playerComponent)
        {
            if (!playerComponent) return;

            var dir = playerComponent.transform.position - component.transform.position;
            component.LookDirection = dir.x < 0 ?
                UsefulEnums.HorizontalDirection.Left :
                UsefulEnums.HorizontalDirection.Right;
        }

        public override void Register(JumpingJackMovementComponent component)
        {
            _player.Where(playerComponent => playerComponent)
                .Select(playerComponent => new {playerComponent, component})
                .Subscribe(obj => StartRegistration(obj.playerComponent, obj.component))
                .AddTo(component);
        }

        private void StartRegistration(PlayerComponent playerComponent, JumpingJackMovementComponent component)
        {
            component.FixedUpdateAsObservable()
                .Subscribe(unit =>
                {
                    var physics = component.GetComponent<OldschoolPhysicComponent>();

                    if (Input.GetButtonDown("Jump") && physics.IsGrounded.Value)
                    {
                        physics.Velocity.Value = new Vector2(physics.Velocity.Value.x, playerComponent.JumpTakeofSpeed);
                    }
                }).AddTo(playerComponent);
        }
    }
}