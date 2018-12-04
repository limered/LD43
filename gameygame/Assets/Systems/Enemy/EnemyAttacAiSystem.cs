using SystemBase;
using Systems.Combat;
using Systems.Combat.Actions;
using Systems.Enemy.Components;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Enums;

namespace Systems.Enemy
{
    public enum DirectionProvider
    {
        None, AimAtPlayer, PatrolMovement
    }

    [GameSystem(typeof(ProjectileSystem))]
    public class EnemyAttacAiSystem : GameSystem<FussiladesShootingComponent>
    {
        public override void Register(FussiladesShootingComponent component)
        {
            component.UpdateAsObservable()
                .Select(unit => component)
                .Subscribe(OnUpdate)
                .AddTo(component);

            PrepareNewFusillade(component);
        }

        private static bool CanShoot(FussiladesShootingComponent component)
        {
            return component.DeltaTimeSinceLastShot >= component.ShotCooldown;
        }

        private static bool FusilladeFinished(FussiladesShootingComponent component)
        {
            return component.ShotCounter >= component.NumberOfShotsPerFusillade;
        }

        private static void OnUpdate(FussiladesShootingComponent obj)
        {
            var shootDirection = obj.DirectionProvider == DirectionProvider.PatrolMovement
                ? obj.GetComponent<PatrolMovementComponent>().MovementDirection
                : obj.DirectionProvider == DirectionProvider.AimAtPlayer
                    ? obj.GetComponent<AimAtPlayerComponent>().LookDirection
                    : UsefulEnums.HorizontalDirection.Left;

            obj.DeltaTimeSinceLastShot += Time.deltaTime;

            if (CanShoot(obj))
            {
                Shoot(obj, shootDirection);
            }
        }

        private static void PrepareNewFusillade(FussiladesShootingComponent component)
        {
            component.ShotCounter = 0;
            component.ShotCooldown = component.TimeBetweenFussilades;
            component.NumberOfShotsPerFusillade = Random.Range(2, 6);
        }

        private static void Shoot(FussiladesShootingComponent component, UsefulEnums.HorizontalDirection dir)
        {
            MessageBroker.Default.Publish(new CombatActShoot
            {
                Direction = new Vector2((float)dir, 0),
                Shooter = component.gameObject
            });

            component.ShotCounter++;
            component.DeltaTimeSinceLastShot = 0;

            if (FusilladeFinished(component))
            {
                PrepareNewFusillade(component);
            }
            else
            {
                component.ShotCooldown = component.TimeBetweenFusilladeShots;
            }
        }
    }
}