using System;
using SystemBase;
using Systems.GameState.Messages;
using Systems.GameState.States;
using UniRx;
using Utils;
using Object = UnityEngine.Object;

namespace Systems.Player
{
    [GameSystem]
    public class PlayerSystem : GameSystem<PlayerSpawnerComponent, PlayerComponent>
    {
        public override void Register(PlayerSpawnerComponent component)
        {
            IoC.Game.GameStateContext
                .AfterStateChange
                .Where(states=> states.Item1.GetType() == typeof(StartScreen) && states.Item2.GetType() == typeof(Running))
                .Subscribe(state=> SpawnPlayer(component));

            MessageBroker.Default.Publish(new GameMsgStart());
        }

        private static void SpawnPlayer(PlayerSpawnerComponent component)
        {
            Object.Instantiate(component.PlayerPrefab, component.transform.position,
                component.transform.localRotation);
        }

        public override void Register(PlayerComponent component)
        {
            
        }
    }
}
