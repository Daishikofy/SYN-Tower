using TOWER.Components;
using UnityEngine;

namespace TOWER
{
    [RequireComponent(typeof(HealthComponent))]
    public class BaseController : MonoBehaviour
    {
        [SerializeField]
        public HealthComponent healthComponent;
        
        private void Awake()
        {
            healthComponent.onDefeated.AddListener(OnDefeated);
        }

        private void OnDefeated()
        {
            Debug.Log("GAME OVER");
        }
    }
}