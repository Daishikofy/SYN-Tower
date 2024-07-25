using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace TOWER
{
    [Serializable]
    public class TowerStats
    {
        public int level;
        public int damage;
        public float shootCooldown;
        public float range;

        public TextMeshProUGUI levelText;
        public TextMeshProUGUI damageText;
        public TextMeshProUGUI rateText;
        public Transform rangeCircle;

        public void Initialize()
        {
            UpdateUI();
        }

        public void LevelUp()
        {
            level++;
            if (level % 4 == 0)
            {
                damage++;
            }
            shootCooldown -= shootCooldown/4f;
            range += 0.50f;
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            levelText.text = level.ToString();
            damageText.text = damage.ToString();
            rateText.text = shootCooldown.ToString("0.00");
            rangeCircle.transform.localScale = Vector3.one * range;
        }
    }
}