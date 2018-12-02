using System;
using SystemBase;
using Systems.GameState.Physics;
using Systems.Health.Actions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Combat
{
    [GameSystem(typeof(OldschoolPhysicSystem))]
    public class CollisionDamageSustem : GameSystem<CollisionDamageComponent>
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
                if (ds.gameObject.GetComponent<CollisionDamageRecieverComponent>())
                {
                    MessageBroker.Default.Publish(new HealthActSubtract
                    {
                        CanKill = true,
                        Target = ds.gameObject,
                        Amount = component.DamageToPlayer
                    });
                }
            };
        }
    }
}