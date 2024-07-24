using TMPro;
using UnityEngine;

namespace TOWER
{
    public class DebugInfoPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyText;
        [SerializeField] private TextMeshProUGUI towerCountText;
        [SerializeField] private TextMeshProUGUI lifePointsText;

        public void UpdateCurrency(int value)
        {
            currencyText.text = value.ToString();
        }

        public void UpdateTowerCount(int value)
        {
            towerCountText.text = value.ToString();
        }

        public void UpdateLifePoints(int value)
        {
            lifePointsText.text = value.ToString();
        }
    }
}