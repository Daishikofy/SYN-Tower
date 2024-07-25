using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace TOWER
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject gameOverPanel;
        public TextMeshProUGUI buildNumberText;

        private void Awake()
        {
            gameObject.SetActive(false);
            victoryPanel.SetActive(false);
            gameOverPanel.SetActive(false);
        }

        public void EnableVictoryPanel()
        {
            gameObject.SetActive(true);
            victoryPanel.SetActive(true);
        }

        public void EnableGameOverPanel()
        {
            gameObject.SetActive(true);
            gameOverPanel.SetActive(true);
        }

        public void DisablePanel()
        {
            gameObject.SetActive(false);
            victoryPanel.SetActive(false);
            gameOverPanel.SetActive(false);
        }

        public void OnRestartPressed()
        {
            GameManager.Instance.Restart();
        }
    }
}