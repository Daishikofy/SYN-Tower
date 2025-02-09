using System;
using UnityEngine;
using UnityEngine.Events;

namespace TOWER.Components
{
    [Serializable]
    public class HealthComponent : MonoBehaviour
    {
        public int maxHealth = 10;
        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                if (_currentHealth != value)
                {
                    _currentHealth = value;
                    onHealthChanged.Invoke(_currentHealth);
                }
            }
        }
        private int _currentHealth;
        
        [Header("Callbacks")]
        public UnityEvent<int> onHealthChanged;
        public UnityEvent onDefeated;
        
       private void Awake()
       {
           _currentHealth = maxHealth;
       }

       public void Damage(int damages)
        {
            if (IsAlive())
            {
                CurrentHealth -= damages;
                if (CurrentHealth <= 0)
                {
                    onDefeated.Invoke();
                }
            }
        }

        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }
    }
}