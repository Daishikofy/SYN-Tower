using TOWER.Components;
using UnityEngine;
using UnityEngine.UI;

namespace TOWER
{
    [RequireComponent(typeof(HealthComponent))]
    public class BaseController : MonoBehaviour
    {
        [SerializeField]
        public HealthComponent healthComponent;

        [SerializeField] private Slider healthBar;
        private void Awake()
        {
            healthComponent.onDefeated.AddListener(OnDefeated);
            healthBar.maxValue = healthComponent.maxHealth;
            healthBar.value = healthComponent.CurrentHealth;
        }

        private void OnDefeated()
        {
            GameManager.Instance.OnGameOver();
        }

        public void UpdateHealthBar(int value)
        {
            healthBar.value = value;
        }
    }
}