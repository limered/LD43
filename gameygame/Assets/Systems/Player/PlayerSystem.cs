﻿using System;
using SystemBase;
using Systems.Combat;
using Systems.Combat.Events;
using Systems.GameState.Messages;
using Systems.GameState.States;
using Systems.Health;
using Systems.Health.Actions;
using Systems.Health.Events;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;
using Assets.Systems.Sound;

namespace Systems.Player
{
    [GameSystem]
    public class PlayerSystem : GameSystem<PlayerSpawnerComponent, PlayerComponent>
    {
        public override void Register(PlayerSpawnerComponent component)
        {
            IoC.Game.GameStateContext
                .AfterStateChange
                .Where(states => states.Item2.GetType() == typeof(Running))
                .Subscribe(state => SpawnPlayer(component));
        }

        private static void SpawnPlayer(PlayerSpawnerComponent component)
        {
            Object.Instantiate(component.PlayerPrefab, component.transform.position,
                component.transform.localRotation);
        }

        public override void Register(PlayerComponent component)
        {
            component.GetComponent<HealthComponent>()
                .CurrentHealth
                .Subscribe(OnHealthLost(component))
                .AddTo(component);

            component.UpdateAsObservable()
                .Subscribe(unit => { UpdatePlayerSize(component); })
                .AddTo(component);

            MessageBroker.Default.Receive<CombatEvtProjectileHit>()
                .Where(hit => hit.HitData.rigidbody.gameObject == component.gameObject)
                .Where(hit => component.LastHitTimestamp < Time.realtimeSinceStartup - component.TimBetweenHits)
                .Subscribe(hit =>
                {
                    component.LastHitTimestamp = Time.realtimeSinceStartup;

                    "PlayerHit".Play();
                    var reciever = component.gameObject.GetComponent<CollisionDamageRecieverComponent>();
                    if (reciever)
                    {
                        reciever.RecievedDamage.Execute();
                    }

                    MessageBroker.Default.Publish(new HealthActSubtract
                    {
                        CanKill = true,
                        Target = component.gameObject,
                        Amount = hit.Projectile.Damage
                    });
                })
                .AddTo(component);

            MessageBroker.Default.Receive<HealthEvtDied>()
                .Where(died => died.Target == component.gameObject)
                .Subscribe(died => {

                    "EnemyDeath".Play();

                    Object.Destroy(component.gameObject);
                    MessageBroker.Default.Publish(new GameMsgEnd());
                    MessageBroker.Default.Publish(new GameMsgRestart());
                })
                .AddTo(component);

            component.UpdateAsObservable()
                .Where(unit => component.transform.position.y < -5)
                .Subscribe(unit => {
                    MessageBroker.Default.Publish(new HealthActSubtract
                    {
                        CanKill = true,
                        Target = component.gameObject,
                        Amount = 200
                    });
                })
                .AddTo(component);
        }

        private static Action<float> OnHealthLost(PlayerComponent component)
        {
            return f =>
            {
                var health = component.GetComponent<HealthComponent>();
                var scale = f / health.MaxHealth;
                component.CurrentSize.Value = Mathf.Lerp(component.SmallSize, component.FullSize, scale);
            };
        }

        private void UpdatePlayerSize(PlayerComponent component)
        {
            component.gameObject.transform.localScale = new Vector3(
                component.CurrentSize.Value,
                component.CurrentSize.Value, 
                1);
        }
    }
}