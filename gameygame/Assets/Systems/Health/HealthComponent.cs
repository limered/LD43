using SystemBase;
using UniRx;

namespace Systems.Health
{
    public class HealthComponent : GameComponent
    {
        public float StartHealth;
        public float MaxHealth;
        public FloatReactiveProperty CurrentHealth = new FloatReactiveProperty();
    }
}