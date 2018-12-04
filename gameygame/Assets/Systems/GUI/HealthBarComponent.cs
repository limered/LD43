using SystemBase;
using UnityEngine;

namespace Assets.Systems.GUI
{
    public class HealthBarComponent : GameComponent
    {
        public float MaxValue;
        public float TargetValue;
        public float CurrentValue;

        [Range(0.0f, 1.0f)]
        public float CurrentPercentOfBar;

        public float BarMaxLength { get; set; }
    }
}