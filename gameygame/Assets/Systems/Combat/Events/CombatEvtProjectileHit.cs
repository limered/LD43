using UnityEngine;

namespace Systems.Combat.Events
{
    public class CombatEvtProjectileHit
    {
        public ProjectileComponent Projectile;
        public RaycastHit2D HitData;
    }
}
