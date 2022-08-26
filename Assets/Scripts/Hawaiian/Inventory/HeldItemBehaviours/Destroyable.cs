using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class Destroyable : MonoBehaviour
    {
        public UnityEvent destroyed = new();
        
        private void OnDestroy()
        {
            destroyed.Invoke();
        }
    }
}