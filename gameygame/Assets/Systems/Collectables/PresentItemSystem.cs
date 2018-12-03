using System.Collections.Generic;
using SystemBase;
using Systems.Collectables.Events;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Assets.Systems.Sound;

namespace Systems.Collectables
{
    [GameSystem(typeof(CollectableSpawnSystem))]
    public class PresentItemSystem : GameSystem<PresentComponent, PlayerComponent>
    {
        private readonly Dictionary<string, PresentComponent> _presents =
            new Dictionary<string, PresentComponent>();

        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>();

        public override void Register(PresentComponent component)
        {
            if (_presents.ContainsKey(component.Id))
            {
                _presents[component.Id] = component;
            }
            else
            {
                _presents.Add(component.Id, component);
            }

            _player.Where(playerComponent => playerComponent)
                .Select(playerComponent => new {playerComponent, component})
                .Subscribe(obj => ActivateSubscription(obj.playerComponent, obj.component))
                .AddTo(component);
        }

        private static void ActivateSubscription(PlayerComponent playerComponent, PresentComponent component)
        {
            component.OnTriggerEnter2DAsObservable()
                .Where(d => d.gameObject == playerComponent.gameObject)
                .Subscribe(d =>
                {
                    "present".Play();
                    MessageBroker.Default.Publish(new PresentEvtCollected {Present = component});
                    Object.Destroy(component.gameObject);
                })
                .AddTo(playerComponent);
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }
    }
}