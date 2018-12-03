using System;
using SystemBase;
using Systems.GameState.Messages;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.ActionTrigger
{
    [GameSystem(typeof(PlayerSystem))]
    public class ActionTriggerSystem : GameSystem<EndGameTriggerComponent, PlayerComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>();

        public override void Register(EndGameTriggerComponent component)
        {
            _player.Where(playerComponent => playerComponent)
                .Select(playerComponent => new { playerComponent, component })
                .Subscribe(obj => SubscribeToEndgameCollision(obj.playerComponent, obj.component))
                .AddTo(component);
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        private static void SubscribeToEndgameCollision(PlayerComponent playerComponent, EndGameTriggerComponent component)
        {
            component.OnTriggerEnter2DAsObservable()
                .Where(d => d.gameObject == playerComponent.gameObject)
                .Subscribe(d => { MessageBroker.Default.Publish(new GameMsgEnd()); })
                .AddTo(playerComponent);
        }
    }
}