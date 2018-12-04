using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemBase;
using Systems.Health;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Plugins;

namespace Assets.Systems.GUI
{
    [GameSystem(typeof(PlayerSystem))]
    public class GUISystem : GameSystem<PlayerComponent, HealthBarComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>();

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        public override void Register(HealthBarComponent component)
        {
            _player.WhereNotNull()
                .Select(playerComponent => new {playerComponent, component})
                .Subscribe(obj => RegisterHealthBarComponent(obj.playerComponent, obj.component))
                .AddTo(component);

            component.BarMaxLength = component.GetComponent<RectTransform>().sizeDelta.x;

            component.UpdateAsObservable()
                .Where(unit => component.MaxValue > 0)
                .Subscribe(unit =>
                {
                    // UpdateValues
                    var delta = component.TargetValue - component.CurrentValue;
                    var step = delta * 0.7f;
                    component.CurrentValue = component.CurrentValue + step;
                    component.CurrentPercentOfBar = component.CurrentValue / component.MaxValue;

                    // UpdateGraphics
                    var transform = component.GetComponent<RectTransform>();
                    transform.sizeDelta = new Vector2(component.BarMaxLength * component.CurrentPercentOfBar, transform.sizeDelta.y);
                })
                .AddTo(component);
        }

        private void RegisterHealthBarComponent(PlayerComponent playerComponent, HealthBarComponent component)
        {
            var health = playerComponent.GetComponent<HealthComponent>();
            component.MaxValue = health.MaxHealth;
            health.CurrentHealth.Subscribe(f => { component.TargetValue = f; }).AddTo(playerComponent);
        }
    }
}
