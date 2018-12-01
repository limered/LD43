using SystemBase;
using Systems.GameState.Physics;
using UnityEngine;

namespace Systems.Player
{
    [RequireComponent(typeof(OldschoolPhysicComponent))]
    public class PlayerComponent : GameComponent
    {
        public float JumpTakeofSpeed = 7;
        public float MovementMaxSpeed = 7;
    }
}