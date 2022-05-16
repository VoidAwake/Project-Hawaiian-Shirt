using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    public class HeldItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent itemUsed;
        
        public void Use() {
            itemUsed.Invoke();
        }
    }
}