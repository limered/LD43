using SystemBase;
using SystemBase.StateMachineBase;
using Systems.GameState.States;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Systems.Enemy
{
    [GameSystem]
    public class EnemySystem : GameSystem<EnemySpawnerComponent>
    {
        public override void Init()
        {
            IoC.Game.GameStateContext.AfterStateChange.Where(IsStateChangeFromStartScreenToRunning)
                .Subscribe(_ => AddEnemyTriggerArea());
        }

        public override void Register(EnemySpawnerComponent component)
        {
            IoC.Game.GameStateContext.AfterStateChange.Where(IsStateChangeFromStartScreenToRunning)
                .Subscribe(_ => SpawnEnemy(component));
        }

        private static bool IsStateChangeFromStartScreenToRunning(Tuple<BaseState<Game>, BaseState<Game>> states)
        {
            return states.Item1.GetType() == typeof(StartScreen) && states.Item2.GetType() == typeof(Running);
        }

        private static void SpawnEnemy(EnemySpawnerComponent component)
        {
            Object.Instantiate(component.EnemyPrefab, component.transform.position, component.transform.localRotation);
        }

        private static void AddEnemyTriggerArea()
        {
            var triggerArea = new GameObject("TriggerArea");
            var collider = triggerArea.gameObject.AddComponent<BoxCollider2D>();
            triggerArea.gameObject.AddComponent<Rigidbody2D>();
            collider.isTrigger = true;
            triggerArea.UpdateAsObservable().Subscribe(_ => UpdateCollider(triggerArea, collider));
            collider.OnTriggerEnter2DAsObservable().Subscribe(OnTriggerEnter2D);
        }

        private static void UpdateCollider(GameObject triggerArea, BoxCollider2D collider)
        {
            var camera = UnityEngine.Camera.current;
            if (camera == null)
            {
                return;
            }

            var camPosition = camera.transform.position;
            triggerArea.transform.position = new Vector3(camPosition.x, camPosition.y, 0);
            var screenWidth = camera.orthographicSize * Screen.width / Screen.height * 2;
            collider.size = new Vector2(screenWidth, camera.orthographicSize * 2);
        }

        private static void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.gameObject.GetComponent<EnemyComponent>();
            if (enemy != null)
            {
                enemy.IsActive = true;
            }
        }
    }
}
