using System;
using SystemBase;
using Systems.GameState.Physics;
using Systems.Health.Actions;
using Assets.Systems.Sound;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Combat
{
    [GameSystem(typeof(OldschoolPhysicSystem))]
    public class CollisionDamageSustem : GameSystem<CollisionDamageComponent, CollisionDamageRecieverComponent>
    {
        public override void Register(CollisionDamageComponent component)
        {
            component.OnCollisionEnter2DAsObservable()
                .Subscribe(OnCollisionDetected(component))
                .AddTo(component);
        }

        private Action<Collision2D> OnCollisionDetected(CollisionDamageComponent component)
        {
            return ds =>
            {
                var reciever = ds.gameObject.GetComponent<CollisionDamageRecieverComponent>();
                if (reciever)
                {
                    reciever.RecievedDamage.Execute();
                    "PlayerHit".Play();

                    MessageBroker.Default.Publish(new HealthActSubtract
                    {
                        CanKill = true,
                        Target = ds.gameObject,
                        Amount = component.DamageToPlayer
                    });
                }
            };
        }

        public override void Register(CollisionDamageRecieverComponent component)
        {
            component.RecievedDamage.Subscribe(unit =>
            {
                component.Shake();
            }).AddTo(component);
        }
    }
}