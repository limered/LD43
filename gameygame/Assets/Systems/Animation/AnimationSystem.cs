using System;
using SystemBase;
using Systems.GameState.Messages;
using Systems.GameState.States;
using Systems.Health;
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
    public class AnimationSystem : GameSystem<GeorgeAnimationComponent>
    {
        public override void Register(GeorgeAnimationComponent component)
        {
            var physics = component.GetComponent<OldschoolPhysicComponent>();
            var player = component.GetComponent<PlayerComponent>();
            var health = component.GetComponent<HealthComponent>();

            //ANIMATION: Walking
            physics.TargetVellocity.Subscribe(v => player.Animator.SetBool("isWalking", v.x != 0)).AddTo(component);

            //ANIMATION: player got hit 
            health.CurrentHealth.Pairwise()
                                .Where(x => x.Previous > x.Current)
                                .Subscribe(x => player.Animator.SetTrigger("gotHit"))
                                .AddTo(component);


        }
    }
}