using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace TOWER
{
    public class SpawnPointController : MonoBehaviour
    {
        public Transform target;
        
        private SpawnSequence[] _sequence;
        
        private float _spawnTimer;
        private int _currentItem;
        private int _currentSequence;

        private List<Vector2> _pathToTarget;

        public void SetupWave(SpawnSequence[] spawnSequences)
        {
            _sequence = spawnSequences;
            _currentItem = 0;
            _currentSequence = 0;
            _spawnTimer = 0f;
        }

        private void Start()
        {
            UpdatePathFinding();
        }
        
        public void UpdatePathFinding()
        {
            Pathfinder pathfinder = GameManager.Instance.pathfinderManager;
            _pathToTarget = pathfinder.ShortestPath(transform.position,
                target.transform.position,
                pathfinder.PATHOFFSET);
        }

        private void Update()
        {
            if (_pathToTarget.Count == 0)
            {
                return;
            }
            if (_currentSequence < _sequence?.Length)
            {
                _spawnTimer += Time.deltaTime;
                if (_spawnTimer >= _sequence[_currentSequence].spawnRate)
                {
                    if (_currentItem < _sequence[_currentSequence].amount)
                    {
                        Spawn();
                        _currentItem++;
                        _spawnTimer = 0f;
                    }
                    else
                    {
                        _currentSequence++;
                        _currentItem = 0;
                    }
                }
            }
        }

        private void Spawn()
        {
            EnemyController enemy = Instantiate(_sequence[_currentSequence].enemyPrefab, 
                    transform.position, 
                    quaternion.identity, 
                    transform);
                
                enemy.Initialize(target, _pathToTarget);
                GameManager.Instance.OnEnemySpawned(enemy);
        }
        private void OnDrawGizmos()
        {
            if (_pathToTarget == null)
                return;
            for (int i = 1; i < _pathToTarget.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_pathToTarget[i-1], _pathToTarget[i]);
            }
        }
    }
}