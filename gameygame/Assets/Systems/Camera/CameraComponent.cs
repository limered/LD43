using SystemBase;
using UnityEngine;

namespace Systems.Camera
{
    public class CameraComponent : GameComponent
    {
        public GameObject Player { get; set; }

        public float AnimationModifier = 1;

        public CamModus CurrentCamModus;

        public float TriggerDIstance = 5;
        public float FixPointDistance = 3;
    }
}