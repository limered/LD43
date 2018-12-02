using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Combat
{
    public class ShooterComponent : GameComponent
    {
        public ReactiveCommand<Vector2> ShootCommand = new ReactiveCommand<Vector2>();

        public GameObject ProjectilePrefab;
    }
}