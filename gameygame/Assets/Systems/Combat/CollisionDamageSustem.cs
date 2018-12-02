using System;
using SystemBase;
using Systems.GameState.Physics;
using Systems.Physics;
using UniRx;
using UnityEngine;

namespace Systems.Combat
{
    [GameSystem(typeof(OldschoolPhysicSystem))]
    public class CollisionDamageSustem : GameSystem<CollisionDamageComponent>
    {
        public override void Register(CollisionDamageComponent component)
        {
            component.GetComponent<OldschoolPhysicComponent>()
                .CollisionDetected
                .Subscribe(OnCollisionDetected(component))
                .AddTo(component);
        }

        private Action<RaycastHit2D[]> OnCollisionDetected(CollisionDamageComponent component)
        {
            return ds =>
            {

            };
        }
    }

    [RequireComponent(typeof(OldschoolPhysicComponent))]
    public class CollisionDamageComponent : GameComponent
    {
        public float DamageToPlayer;
    }
}
