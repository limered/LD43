using SystemBase;
using Systems.Physics;
using Systems.Player;
using UniRx;
using UnityEngine;

namespace Systems.Animation
{
    public class CrumbleComponent : GameComponent
    {
        public CrumbleReason Reason;
        public ParticleSystem CrumblePrefab;
    }

    public enum CrumbleReason
    {
        WhenDie
    }
}