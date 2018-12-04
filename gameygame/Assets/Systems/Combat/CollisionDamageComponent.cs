using SystemBase;
using Systems.Physics;
using UnityEngine;

namespace Systems.Combat
{
    [RequireComponent(typeof(OldschoolPhysicComponent))]
    public class CollisionDamageComponent : GameComponent
    {
        public float DamageToPlayer;
    }
}