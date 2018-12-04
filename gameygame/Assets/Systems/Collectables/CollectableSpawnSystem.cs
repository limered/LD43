using SystemBase;
using Systems.GameState.States;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems.Collectables
{
    [GameSystem]
    public class CollectableSpawnSystem : GameSystem<OneTimeCollectableSpawner>
    {
        public override void Register(OneTimeCollectableSpawner component)
        {
            IoC.Game.GameStateContext.AfterStateChange
                .Where(tuple => tuple.Item1.GetType() == typeof(StartScreen) &&
                                tuple.Item2.GetType() == typeof(Running))
                .Subscribe(tuple =>
                    {
                        Object.Instantiate(component.ItemToSpawn, 
                            component.transform.position, 
                            Quaternion.identity,
                            component.transform);
                    })
                .AddTo(component);
        }
    }
}
