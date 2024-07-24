using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace TOWER
{
    [Serializable]
    public class TOW_Stats
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
            damage++;
            shootCooldown -= 0.5f;
            range += 0.10f;
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            levelText.text = level.ToString();
            damageText.text = damage.ToString();
            rateText.text = shootCooldown.ToString(CultureInfo.InvariantCulture);
            rangeCircle.transform.localScale = Vector3.one * range;
        }
    }
}