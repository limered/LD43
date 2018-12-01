using SystemBase;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Camera
{
    [GameSystem(typeof(PlayerSystem))]
    public class CameraSystem : GameSystem<CameraComponent, PlayerComponent>
    {
        private ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);

        public override void Register(CameraComponent component)
        {
            _player
                .Where(playerComponent => playerComponent != null)
                .Subscribe(playerComponent =>
                {
                    component.Player = playerComponent.gameObject;
                    component
                        .FixedUpdateAsObservable()
                        .Subscribe(unit => MoveCamera(component))
                        .AddTo(component);
                });
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        private static void MoveCamera(CameraComponent component)
        {
            var playerPosition = component.Player.transform.position;
            var camPosition = component.transform.position;
            var distance = playerPosition - camPosition;

            //var newX = CalculateNewPosition(component.FreezeX, distance.x, camPosition.x, component.ThresholdDistance,
            //    component.AnimationModifier);

            var newX = camPosition.x;

            if (component.CurrentCamModus == CamModus.Left)
            {
                var triggerPoint = camPosition.x + component.TriggerDIstance;
                var movementPoint = camPosition.x + component.FixPointDistance;

                if (playerPosition.x < movementPoint)
                {
                    var distancex = playerPosition.x - movementPoint;
                    newX = camPosition.x + distancex * component.AnimationModifier * Time.fixedDeltaTime;
                }

                if (playerPosition.x > triggerPoint)
                {
                    component.CurrentCamModus = CamModus.Right;
                }
            }
            else
            {
                var triggerPoint = camPosition.x - component.TriggerDIstance;
                var movementPoint = camPosition.x - component.FixPointDistance;

                if (playerPosition.x > movementPoint)
                {
                    var distancex = playerPosition.x - movementPoint;
                    newX = camPosition.x + distancex * component.AnimationModifier * Time.fixedDeltaTime;
                }

                if (playerPosition.x < triggerPoint)
                {
                    component.CurrentCamModus = CamModus.Left;
                }
            }

            component.transform.position = new Vector3(newX, 
                component.transform.position.y, 
                component.transform.position.z);
        }

        private static float CalculateNewPosition(bool calculationFrozen, float distance, float posiiton, float threshold, float animationDamping)
        {
            if (calculationFrozen || Mathf.Abs(distance) < threshold) return posiiton;
            return distance < 0
                ? posiiton + (distance + threshold) * animationDamping
                : posiiton + (distance - threshold) * animationDamping;
        }
    }

    public enum CamModus
    {
        Left = 1,
        Right = 2
    }
}
