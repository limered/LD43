using SystemBase;
using Systems.Combat;
using Systems.Combat.Actions;
using Systems.GameState.Physics;
using Systems.Physics;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Enemy.Lagacy
{
    [GameSystem(typeof(PlayerSystem), typeof(OldschoolPhysicSystem), typeof(ProjectileSystem), typeof(EnemySystem))]
    public class EnemyControlSystem : GameSystem<EnemyComponent, PlayerComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);
        private float _deltaTimeSinceLastShot;

        public override void Register(EnemyComponent component)
        {
            _player.Where(playerComponent => playerComponent != null).Subscribe(playerComponent =>
            {
                component.UpdateAsObservable().Subscribe(_ => AttackPlayer(component));
            });
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        private void AttackPlayer(EnemyComponent component)
        {
            if (!component.IsActive)
            {
                return;
            }

            MoveTowardsPlayer(component);
            ShootAtPlayer(component);
        }

        private void MoveTowardsPlayer(EnemyComponent component)
        {
            var physics = component.GetComponent<OldschoolPhysicComponent>();
            var move = Vector2.zero;
            move.x = (_player.Value.transform.position - component.transform.position).x;
            physics.TargetVellocity.Value = move;
        }

        private void ShootAtPlayer(EnemyComponent component)
        {
            if (_deltaTimeSinceLastShot < component.ShootingDelay)
            {
                _deltaTimeSinceLastShot += Time.deltaTime;
                return;
            }

            var direction = (_player.Value.transform.position - component.transform.position).x;
            var shootDirection = direction < 0 ? Vector2.left : Vector2.right;
            MessageBroker.Default
                .Publish(new CombatActShoot
                {
                    Direction = shootDirection,
                    Shooter = component.gameObject
                });
            _deltaTimeSinceLastShot = 0;
        }
    }
}
