using SystemBase;
using Systems.GameState.Physics;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Player
{
    [GameSystem(typeof(PlayerSystem), typeof(OldschoolPhysicSystem))]
    public class PlayerControlSystem : GameSystem<PlayerComponent>
    {
        public override void Register(PlayerComponent component)
        {
            component
                .GetComponent<OldschoolPhysicComponent>()
                .UpdateAsObservable()
                .Subscribe(_=>ComputeVelocity(component))
                .AddTo(component);
        }

        private static void ComputeVelocity(PlayerComponent component)
        {
            var physics = component.GetComponent<OldschoolPhysicComponent>();
            var move = Vector2.zero;

            move.x = Input.GetAxis("Horizontal") * component.MovementMaxSpeed;

            if (Input.GetButtonDown("Jump") && physics.IsGrounded.Value)
            {
                physics.Velocity.Value = new Vector2(physics.Velocity.Value.x, component.JumpTakeofSpeed);
            }
            else if(Input.GetButtonUp("Jump"))
            {
                if (physics.Velocity.Value.y > 0)
                {
                    physics.Velocity.Value = new Vector2(physics.Velocity.Value.x, physics.Velocity.Value.y * 0.5f);
                }
            }

            physics.TargetVellocity.Value = move;
        }
    }
}
