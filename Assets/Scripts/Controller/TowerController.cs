using System.Collections.Generic;
using UnityEngine;

namespace TOWER
{
    public class TowerController : MonoBehaviour
    {
        [Header("Attack setup")] public ProjectileController projectilePrefab;
        [SerializeField] private Transform projectilesHolder;
        public string targetTag = "";

        [Header("Tower Stats")] [SerializeField]
        private TowerStats stats;

        [SerializeField] private GameObject ui;

        [Header("XP info")] public int xpRate = 1;

        public int levelThreshold = 10;

        private float _shootTimer;
        private float _xpPoints;
        private bool _isActive;

        private Transform _playerTransform;

        private void Start()
        {
            _playerTransform = GameManager.Instance.playerTransform;
            stats.Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            _shootTimer += Time.deltaTime;
            if (_shootTimer >= stats.shootCooldown)
            {
                SpawnProjectile();
                _shootTimer = 0f;
            }

            if (_isActive)
            {
                _xpPoints += xpRate * Time.deltaTime;
                if (_xpPoints >= levelThreshold)
                {
                    stats.LevelUp();
                    levelThreshold += (int) (levelThreshold * 1.2f);
                }
            }

            ui.SetActive(Vector2.Distance(transform.position, _playerTransform.position) < 1.0f);
        }

        public void EnableTower(Vector2 position)
        {
            gameObject.transform.position = position;
            gameObject.SetActive(true);
        }

        public void DisableTower()
        {
            gameObject.SetActive(false);
            foreach (Transform projectile in projectilesHolder)
            {
                Destroy(projectile.gameObject);
            }
        }

        private void SpawnProjectile()
        {
            //Todo: Pooling system
            Transform parentTransform = transform;
            List<EnemyController> enemiesInRange =
                GameManager.Instance.GetEnemiesInRange(parentTransform.position, stats.range);
            if (enemiesInRange.Count > 0)
            {
                ProjectileController projectile = Instantiate(projectilePrefab, parentTransform.position,
                    Quaternion.identity, projectilesHolder);

                int closestTarget = 0;
                float closestDistance =
                    Vector2.Distance(transform.position, enemiesInRange[closestTarget].transform.position);
                for (int i = 1; i < enemiesInRange.Count; i++)
                {
                    float distance = Vector2.Distance(transform.position, enemiesInRange[i].transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = i;
                    }
                }

                projectile.Initialize(enemiesInRange[closestTarget].transform, stats.range, stats.damage, targetTag);
                _isActive = true;
            }
            else
            {
                _isActive = false;
            }
        }
    }
}