using System;
using System.Collections.Generic;
using UnityEngine;

namespace TOWER
{
    [Serializable]
    public class SpawnPointManager
    {
        [SerializeField] private List<SpawnPointController> spawnPoints;

        private EnemyWave _currentWave;
        
        private List<EnemyController> _enemies = new List<EnemyController>();
        private int _enemyCount;

        private int _defeatedEnemiesCount;
        private int _enemiesToDefeatCount;

        public void InitializeCurrentWave(EnemyWave wave)
        {
            _currentWave = wave;
            
            foreach (SpawnPointSequences spawnPointSequences in _currentWave.spawnPointsSequences)
            {
                spawnPoints[spawnPointSequences.spawnPointId].SetupWave(spawnPointSequences.spawnSequences);
            }
            
            _enemies.Clear();
            _enemyCount = 0;
            _defeatedEnemiesCount = 0;
            _enemiesToDefeatCount = _currentWave.GetEnemiesInWaveCount();
        }
        
        public List<EnemyController> GetEnemiesInRange(Vector2 position, float range)
        {
            List<EnemyController> enemiesInRange = new List<EnemyController>();
            for (int i = 0; i < _enemyCount; i++)
            {
                EnemyController enemy = _enemies[i];
                if (Vector2.Distance(position, enemy.transform.position) <= range)
                {
                    enemiesInRange.Add(enemy);
                }
            }

            return enemiesInRange;
        }

        public void OnEnemySpawned(EnemyController enemy)
        {
            if (_enemies.Count > _enemyCount)
            {
                _enemies[_enemyCount] = enemy;
            }
            else
            {
                _enemies.Add(enemy);
            }

            _enemyCount++;
        }

        public void OnEnemyDefeated(EnemyController enemy)
        {
            for (int i = 0; i < _enemyCount; i++)
            {
                if (i != _enemyCount - 1 && _enemies[i] == enemy)
                {
                    _enemies[i] = _enemies[_enemyCount - 1];
                }
            }

            _enemyCount -= 1;
            _defeatedEnemiesCount += 1;
            if (_defeatedEnemiesCount >= _enemiesToDefeatCount)
            {
                GameManager.Instance.OnWaveEnded();
            }
        }
        
        public void UpdatePathFinding()
        {
            foreach (SpawnPointController spawnPoint in spawnPoints)
            {
                spawnPoint.UpdatePathFinding();
            }
            foreach (EnemyController enemy in _enemies)
            {
                enemy.UpdatePathFinding();
            }
        }
    }
}