using System;
using UnityEngine;

namespace TOWER
{
    public class TOW_UIManager : MonoBehaviour
    {
        private static TOW_UIManager _instance;
        public static TOW_UIManager Instance => _instance;

        [SerializeField] private DebugInfoPanel debugInfoPanel;
        [SerializeField] private EndGamePanel endGamePanel;

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
            
            var request = Resources.LoadAsync("Build", typeof(BuildNumberData));
            request.completed += OnLoad;
        }
        
        private void OnLoad(AsyncOperation operation)
        {
            var request = (ResourceRequest) operation;
            var buildNumberData = (BuildNumberData) request.asset; 
            endGamePanel.buildNumberText.text = buildNumberData.BuildNumber;
        }

        private void Start()
        {
            endGamePanel.DisablePanel();
        }

        public void UpdateCurrency(int value)
        {
            debugInfoPanel.UpdateCurrency(value);
        }
        
        public void UpdateTowerCount(int value)
        {
            debugInfoPanel.UpdateTowerCount(value);
        }

        public void ShowGameOver()
        {
            endGamePanel.EnableGameOverPanel();
        }

        public void ShowVictory()
        {
            endGamePanel.EnableVictoryPanel();
        }
    }
}