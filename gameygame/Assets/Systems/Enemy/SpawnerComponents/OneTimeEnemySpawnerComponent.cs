using SystemBase;
using UnityEngine;

namespace Systems.Enemy.SpawnerComponents
{
    public class OneTimeEnemySpawnerComponent : GameComponent
    {
        public SpawnerType SpawnerType;

        public GameObject LeftBorder;
        public GameObject Rightborder;

        public GameObject Prefab;
    }
}