using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TOWER
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;

        public Transform playerTransform;

        [Header("Managers")] 
        [SerializeField] private SpawnPointManager spawnPointManager;

        [SerializeField] private TOW_UIManager uiManager;

        public Pathfinder pathfinderManager;
        public MapController mapManager;
        
        [Header("Towers")] 
        [SerializeField]  private TowerController towerPrefab;

        [SerializeField] private int towerPrice = 10;

        [Header("Waves parameters")]
        [SerializeField] private LevelScenario levelScenario;

        private int _currentWaveId;

        private int _currencyAmount = 0;
        public int CurrencyAmount {
            get { return _currencyAmount; }
            set
            {
                _currencyAmount = value;
                uiManager.UpdateCurrency(_currencyAmount);
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
            
            pathfinderManager.Initialize(mapManager);
        }

        private void Start()
        {
            uiManager.UpdateCurrency(_currencyAmount);
            StartPlayingLevel();
        }

        // _ _ _ _ _ LEVEL SCENARIO _ _ _ _ _ _
        private void StartPlayingLevel()
        {
            _currentWaveId = 0;
            spawnPointManager.InitializeCurrentWave(levelScenario.enemyWaves[_currentWaveId]);
        }

        public void OnWaveEnded()
        {
            _currentWaveId++;
            if (_currentWaveId < levelScenario.enemyWaves.Length)
            {
                spawnPointManager.InitializeCurrentWave(levelScenario.enemyWaves[_currentWaveId]);
            }
            else
            {
                OnVictory();
            }
        }

        // _ _ _ _ _ ENEMIES _ _ _ _ _ _
        public List<EnemyController> GetEnemiesInRange(Vector2 position, float range)
        {
            return spawnPointManager.GetEnemiesInRange(position, range);
        }
        

        public void OnEnemyDefeated(EnemyController enemy)
        {
            CurrencyAmount += enemy.currencyDrop;
            spawnPointManager.OnEnemyDefeated(enemy);
        }
        
        public void OnEnemySpawned(EnemyController enemy)
        {
            spawnPointManager.OnEnemySpawned(enemy);
        }
        
        // _ _ _ _ _ TOWERS _ _ _ _ _ _
        public bool CanBuyTower()
        {
            if (CurrencyAmount >= towerPrice)
            {
                CurrencyAmount -= towerPrice;
                return true;
            }

            return false;
        }
        
        // _ _ _ _ _ GAME FLOW _ _ _ _ _ _

        public void OnGameOver()
        {
            Time.timeScale = 0;
            uiManager.ShowGameOver();
        }

        public void OnVictory()
        {
            Time.timeScale = 0;
            uiManager.ShowVictory();
        }

        public void Restart()
        {
            Time.timeScale = 1;
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.name, LoadSceneMode.Single);
        }
        
        // _ _ _ _ _ OTHER _ _ _ _ _ _
        public void UpdatedTilemap()
        {
            spawnPointManager.UpdatePathFinding();
        }
    }
}