using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    public class HeldItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent itemUsed;

        public UnityEvent destroyed = new UnityEvent();
        
        public void Use() {
            itemUsed.Invoke();
        }

        public void Destroy()
        {
           destroyed.Invoke(); 
        }
    }
}