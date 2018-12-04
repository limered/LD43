using SystemBase;
using Systems.Enemy.Components;
using Systems.Enemy.SpawnerComponents;
using Systems.GameState.States;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems.Enemy
{
    [GameSystem]
    public class EnemySpawnerSystem : GameSystem<OneTimeEnemySpawnerComponent, RepeatingEnemySpawnercomponent>
    {
        public override void Register(OneTimeEnemySpawnerComponent component)
        {
            IoC.Game.GameStateContext.AfterStateChange
                .Where(tuple =>
                    tuple.Item1.GetType() == typeof(StartScreen) && tuple.Item2.GetType() == typeof(Running))
                .Subscribe(_ =>
                {
                    if (component.SpawnerType == SpawnerType.Camper)
                    {
                        SpawnCamper(component.Prefab, component.transform.position);
                    }
                    else if (component.SpawnerType == SpawnerType.Patrol)
                    {
                        SpawnPatrol(component.Prefab, component.LeftBorder, component.Rightborder, component.transform.position);
                    }
                })
                .AddTo(component);
        }

        public override void Register(RepeatingEnemySpawnercomponent component)
        {
            throw new System.NotImplementedException();
        }

        private void SpawnCamper(GameObject prefab, Vector3 position)
        {
            Object.Instantiate(prefab, position, Quaternion.identity);
        }

        private void SpawnPatrol(GameObject prefab, GameObject leftBorder, GameObject rightBorder, Vector3 position)
        {
            var go = Object.Instantiate(prefab, position, Quaternion.identity);
            var patrol = go.GetComponent<PatrolMovementComponent>();
            patrol.LeftWaypoint = leftBorder;
            patrol.RightWaypoint = rightBorder;
        }
    }

    public enum SpawnerType
    {
        Camper, Patrol
    }

    public class RepeatingEnemySpawnercomponent : GameComponent
    {
        public SpawnerType SpawnerType;

        public GameObject LeftBorder;
        public GameObject Rightborder;

        public GameObject Prefab;
    }
}
