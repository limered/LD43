using System;
using SystemBase;
using Systems.Enemy;
using Systems.GameState.Messages;
using Systems.GameState.States;
using Systems.Health;
using Systems.Health.Actions;
using Systems.Physics;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Systems.Animation
{
    [GameSystem]
    public class AnimationSystem : GameSystem<GeorgeAnimationComponent, EnemyAnimationComponent, RotationAnimationComponent>
    {
        public override void Register(GeorgeAnimationComponent component)
        {
            var physics = component.GetComponent<OldschoolPhysicComponent>();
            var player = component.GetComponent<PlayerComponent>();
            var health = component.GetComponent<HealthComponent>();

            //ANIMATION: Walking
            physics.TargetVellocity.Subscribe(v => player.Animator.SetBool("isWalking", v.x != 0)).AddTo(component);
            physics.Velocity.Subscribe(v => player.Animator.SetFloat("walkspeed", Mathf.Abs(v.x / player.MovementMaxSpeed))).AddTo(component);

            //ANIMATION: player got hit 
            MessageBroker.Default.Receive<HealthActSubtract>()
                .Where(x => x.CanKill)
                .Subscribe(x => player.Animator.SetTrigger("gotHit"))
                .AddTo(component);
        }

        public override void Register(RotationAnimationComponent component)
        {
            if(component.StartWithRandomRotation)
                component.transform.Rotate(Vector3.forward, UnityEngine.Random.Range(0, 360));

            component.FixedUpdateAsObservable()
                     .Select(x => Time.fixedTime)
                     .Subscribe(x => component.transform.Rotate(Vector3.forward, component.ConstantRotation))
                     .AddTo(component);
        }

        public override void Register(EnemyAnimationComponent component)
        {
            var physics = component.GetComponent<OldschoolPhysicComponent>();
            var enemy = component.GetComponent<EnemyComponent>();
            var health = component.GetComponent<HealthComponent>();

            //ANIMATION: Walking
            physics.TargetVellocity.Subscribe(v => enemy.Animator.SetBool("isWalking", v.x != 0)).AddTo(component);
            physics.Velocity.Subscribe(v => enemy.Animator.SetFloat("walkspeed", Mathf.Abs(v.x / enemy.MovementMaxSpeed))).AddTo(component);

            //ANIMATION: enemy got hit 
            MessageBroker.Default.Receive<HealthActSubtract>()
                .Subscribe(x => enemy.Animator.SetTrigger("gotHit"))
                .AddTo(component);
        }
    }
}