using System;
using UnityEngine;

namespace TOWER
{
    public class TOW_UIManager : MonoBehaviour
    {
        private static TOW_UIManager _instance;
        public static TOW_UIManager Instance => _instance;

        [SerializeField] private DebugInfoPanel debugInfoPanel;

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
        }

        public void UpdateCurrency(int value)
        {
            debugInfoPanel.UpdateCurrency(value);
        }
        
        public void UpdateTowerCount(int value)
        {
            debugInfoPanel.UpdateTowerCount(value);
        }

        public void UpdateLifePoint(int value)
        {
            debugInfoPanel.UpdateLifePoints(value);
        }
    }
}