using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class UnityEventHeldItemBehaviour : HeldItemBehaviour
    {
        [SerializeField] private UnityEvent unityEvent = new();
        
        protected override void UseItemActionPerformed(InputAction.CallbackContext value)
        {
            base.UseItemActionPerformed(value);
            
            unityEvent.Invoke();
        }
    }
}