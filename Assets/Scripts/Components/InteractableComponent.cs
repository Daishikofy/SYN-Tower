using UnityEngine;
using UnityEngine.Events;

namespace TOWER
{
    public class InteractableComponent : MonoBehaviour
    {
        public UnityEvent onInteraction;

        public void Interact()
        {
            onInteraction.Invoke();
        }
    }
}