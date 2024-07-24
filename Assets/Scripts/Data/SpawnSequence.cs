using System;

namespace TOWER
{
    [Serializable]
    public class SpawnSequence
    {
        public EnemyController enemyPrefab;
        public int amount;
        public float spawnRate;
    }
}