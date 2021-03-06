﻿using System;
using SystemBase;
using Systems.GameState.States;
using Systems.Health.Actions;
using Systems.Health.Events;
using UniRx;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Systems.Health
{
    [GameSystem]
    public class HealthSystem : GameSystem<HealthComponent, DespawnOnGameOverComponent>
    {
        public override void Register(HealthComponent component)
        {
            component.CurrentHealth.Value = component.StartHealth;

            MessageBroker.Default.Receive<HealthActSubtract>()
                .Where(health => health.Target == component.gameObject)
                .Subscribe(OnHealthSubstract(component))
                .AddTo(component);

            MessageBroker.Default.Receive<HealthActAdd>()
                .Where(health => health.Target == component.gameObject)
                .Subscribe(OnHealthAdd(component))
                .AddTo(component);
        }

        private static Action<HealthActAdd> OnHealthAdd(HealthComponent component)
        {
            return health =>
            {
                if (health.IsFullRestore)
                {
                    component.CurrentHealth.Value = component.MaxHealth;
                    return;
                }

                component.CurrentHealth.Value += health.Amount;
                if (component.CurrentHealth.Value > component.MaxHealth)
                {
                    component.CurrentHealth.Value = component.MaxHealth;
                }
            };
        }

        private static Action<HealthActSubtract> OnHealthSubstract(HealthComponent component)
        {
            return health =>
            {
                component.CurrentHealth.Value -= health.Amount;
                if (component.CurrentHealth.Value < 1 && health.CanKill)
                {
                    MessageBroker.Default.Publish(new HealthEvtDied{Target = component.gameObject});
                }
                else if (component.CurrentHealth.Value < 1 && !health.CanKill)
                {
                    component.CurrentHealth.Value = 1;
                }
            };
        }

        public override void Register(DespawnOnGameOverComponent component)
        {
            IoC.Game.GameStateContext.CurrentState
                .Where(state => state.GetType() == typeof(GameOver))
                .Subscribe(state => Object.Destroy(component.gameObject))
                .AddTo(component);
        }
    }
}
