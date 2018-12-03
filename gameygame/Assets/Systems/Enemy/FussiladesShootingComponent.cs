using SystemBase;

namespace Systems.Enemy
{
    public class FussiladesShootingComponent : GameComponent
    {
        public DirectionProvider DirectionProvider;

        public float TimeBetweenFusilladeShots;
        public float TimeBetweenFussilades;
        public float DeltaTimeSinceLastShot { get; set; }
        public int NumberOfShotsPerFusillade { get; set; }
        public float ShotCooldown { get; set; }
        public int ShotCounter { get; set; }
    }
}