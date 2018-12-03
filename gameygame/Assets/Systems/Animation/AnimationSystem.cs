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
    public class AnimationSystem : GameSystem
    <
        GeorgeAnimationComponent,
        EnemyAnimationComponent,
        RotationAnimationComponent,
        PresentAnimationComponent,
        CrumbleComponent
    >
    {
        public override void Register(GeorgeAnimationComponent component)
        {
            var physics = component.GetComponent<OldschoolPhysicComponent>();
            var player = component.GetComponent<PlayerComponent>();
            var health = component.GetComponent<HealthComponent>();
            var rotateThis = component.WhatShouldRotateWithDirection ? component.WhatShouldRotateWithDirection : player.gameObject.transform;

            //ANIMATION: Walking
            physics.TargetVellocity.Subscribe(v => player.Animator.SetBool("isWalking", v.x != 0)).AddTo(component);
            physics.Velocity.Subscribe(v =>
            {
                player.Animator.SetFloat("walkspeed", Mathf.Abs(v.x));
                rotateThis.localScale = new Vector3(
                    Mathf.Abs(rotateThis.localScale.x) * (float)player.Direction,
                    rotateThis.localScale.y,
                    1);
            }).AddTo(component);

            //ANIMATION: player got hit 
            MessageBroker.Default.Receive<HealthActSubtract>()
                .Where(x => x.CanKill)
                .Subscribe(x => player.Animator.SetTrigger("gotHit"))
                .AddTo(component);
        }

        public override void Register(EnemyAnimationComponent component)
        {
            var physics = component.GetComponent<OldschoolPhysicComponent>();
            var animator = component.GetComponentInChildren<Animator>();
            var enemy = component.GetComponent<Systems.Enemy.Patrol.PatrolEnemyComponent>();
            var rotateThis = component.WhatShouldRotateWithDirection ? component.WhatShouldRotateWithDirection : enemy.gameObject.transform;

            //ANIMATION: Walking
            physics.TargetVellocity.Subscribe(v => enemy.Animator.SetBool("isWalking", v.x != 0)).AddTo(component);
            physics.Velocity.Subscribe(v =>
            {
                enemy.Animator.SetFloat("walkspeed", Mathf.Abs(v.x / enemy.MovementMaxSpeed));
                rotateThis.localScale = new Vector3(
                    Mathf.Abs(rotateThis.localScale.x) * (float) enemy.Direction,
                    rotateThis.localScale.y,
                    1);
            }).AddTo(component);

            //ANIMATION: enemy got hit 
            MessageBroker.Default.Receive<HealthActSubtract>()
                .Subscribe(x => animator.SetTrigger("gotHit"))
                .AddTo(component);
        }

        public override void Register(RotationAnimationComponent component)
        {
            if (component.StartWithRandomRotation)
                component.transform.Rotate(Vector3.forward, UnityEngine.Random.Range(0, 360));

            component.FixedUpdateAsObservable()
                     .Select(x => Time.fixedTime)
                     .Subscribe(x =>
                     {
                         component.transform.Rotate(Vector3.forward, component.ConstantRotation);
                     })
                     .AddTo(component);
        }

        public override void Register(CrumbleComponent component)
        {
            //PARTICLE: crumble when died
            if (component.Reason == CrumbleReason.WhenDie)
            {
                MessageBroker.Default.Receive<Health.Events.HealthEvtDied>()
                    .First(x => x.Target == component.gameObject)
                    .Subscribe(x =>
                    {
                        if (component.CrumblePrefab)
                        {
                            var effect = GameObject.Instantiate(component.CrumblePrefab, component.transform.position, Quaternion.identity);
                            GameObject.Destroy(effect.gameObject, 5);
                        }
                    })
                    .AddTo(component);
            }
        }


        public override void Register(PresentAnimationComponent component)
        {
            component.AnimationSpeed.Subscribe(x =>
            {
                component.GetComponentInChildren<Animator>().SetFloat("speed", x);
            }).AddTo(component);
        }
    }
}