using SystemBase;
using Systems.Physics;
using UnityEngine;

namespace Systems.Player
{
    [RequireComponent(typeof(OldschoolPhysicComponent))]
    public class PlayerComponent : GameComponent
    {
        public float JumpTakeofSpeed = 5;
        public float MovementMaxSpeed = 2;
        public PlayerDirection Direction = PlayerDirection.Right;

        public float HealthLostPerProjectile;
    }
}