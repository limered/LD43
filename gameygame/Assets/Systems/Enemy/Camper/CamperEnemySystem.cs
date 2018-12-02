using SystemBase;
using SystemBase.StateMachineBase;
using Systems.Combat.Actions;
using Systems.GameState.States;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

namespace Systems.Enemy.Camper
{
    [GameSystem(typeof(PlayerSystem))]
    public class CamperEnemySystem : GameSystem<CamperEnemySpawnerComponent, CamperEnemyComponent, PlayerComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);

        private const float TimeBetweenFusilladeShots = .2f;
        private const float TimeBetweenFusillades = 3;

        private float _deltaTimeSinceLastShot;

        private int _numberOfShotsPerFusillade;

        private int _shotCounter;

        private float _shotCooldown = TimeBetweenFusillades;

        public override void Register(CamperEnemySpawnerComponent component)
        {
            IoC.Game.GameStateContext.AfterStateChange.Where(IsStateChangeFromStartScreenToRunning)
                .Subscribe(_ => SpawnEnemy(component));
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        public override void Register(CamperEnemyComponent component)
        {
            _player.Where(playerComponent => playerComponent != null).Subscribe(playerComponent =>
            {
                PrepareNewFusillade();
                component.UpdateAsObservable().Subscribe(_ => ShootAtPlayer(component));
            });
        }

        private static bool IsStateChangeFromStartScreenToRunning(Tuple<BaseState<Game>, BaseState<Game>> states)
        {
            return states.Item1.GetType() == typeof(StartScreen) && states.Item2.GetType() == typeof(Running);
        }

        private static void SpawnEnemy(CamperEnemySpawnerComponent component)
        {
            Object.Instantiate(component.CamperPrefab, component.transform.position, component.transform.localRotation);
        }

        private void ShootAtPlayer(CamperEnemyComponent component)
        {
            _deltaTimeSinceLastShot += Time.deltaTime;

            if (CanShoot())
            {
                Shoot(component);
            }
        }

        private void Shoot(CamperEnemyComponent component)
        {
            var direction = _player.Value.transform.position.x <= component.transform.position.x
                ? Vector2.left
                : Vector2.right;

            MessageBroker.Default.Publish(new CombatActShoot
            {
                Direction = direction,
                Shooter = component.gameObject
            });

            _shotCounter++;
            _deltaTimeSinceLastShot = 0;

            if (FusilladeFinished())
            {
                PrepareNewFusillade();
            }
            else
            {
                _shotCooldown = TimeBetweenFusilladeShots;
            }
        }

        private void PrepareNewFusillade()
        {
            _shotCounter = 0;
            _shotCooldown = TimeBetweenFusillades;
            _numberOfShotsPerFusillade = Random.Range(2, 6);
        }

        private bool FusilladeFinished()
        {
            return _shotCounter >= _numberOfShotsPerFusillade;
        }

        private bool CanShoot()
        {
            return _deltaTimeSinceLastShot >= _shotCooldown;
        }
    }
}
