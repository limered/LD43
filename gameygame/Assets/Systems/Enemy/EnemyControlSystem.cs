using SystemBase;
using UniRx;
using UniRx.Triggers;

namespace Systems.Enemy
{
    public class EnemyControlSystem : GameSystem<EnemyComponent>
    {
        public override void Register(EnemyComponent component)
        {
            component.UpdateAsObservable().Subscribe(_ => AttackPlayer(component));
        }

        private void AttackPlayer(EnemyComponent component)
        {
            if (!component.IsActive)
            {
                return;
            }

            //ToDo: Attack the player
        }
    }
}
