using SystemBase;
using Systems.Combat.Events;
using Systems.Enemy.Components;
using Systems.Health;
using Systems.Health.Actions;
using Systems.Health.Events;
using UniRx;
using UnityEngine;

namespace Systems.Enemy
{
    [GameSystem(typeof(HealthSystem))]
    public class EnemySystem : GameSystem<EnemyComponent>
    {
        public override void Register(EnemyComponent component)
        {
            MessageBroker.Default.Receive<CombatEvtProjectileHit>()
                .Where(hit => hit.HitData.rigidbody.gameObject == component.gameObject)
                .Subscribe(hit =>
                {
                    MessageBroker.Default.Publish(new HealthActSubtract
                    {
                        CanKill = true,
                        Target = component.gameObject,
                        Amount = component.GetComponent<HealthComponent>().CurrentHealth.Value
                    });
                })
                .AddTo(component);

            MessageBroker.Default.Receive<HealthEvtDied>()
                .Where(died => died.Target == component.gameObject)
                .Subscribe(died => Object.Destroy(died.Target))
                .AddTo(component);
        }
    }
}