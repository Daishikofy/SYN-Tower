using System;

namespace TOWER
{
    [Serializable]
    public class EnemyWave
    {
        public SpawnPointSequences[] spawnPointsSequences = {new SpawnPointSequences()};

        public int GetEnemiesInWaveCount()
        {
            int count = 0;

            foreach (SpawnPointSequences spawnPointsSequence in spawnPointsSequences)
            {
                foreach (SpawnSequence sequence in spawnPointsSequence.spawnSequences)
                {
                    count += sequence.amount;
                }
            }

            return count;
        }
    }
}