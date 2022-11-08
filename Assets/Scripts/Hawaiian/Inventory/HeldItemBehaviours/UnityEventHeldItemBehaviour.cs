using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class UnityEventHeldItemBehaviour : HeldItemBehaviour
    {
        [SerializeField] private UnityEvent OnItemPerformed = new();

        [SerializeField] private UnityEvent OnItemCancelled = new();


        protected override void UseItemActionPerformed(InputAction.CallbackContext value)
        {
            base.UseItemActionPerformed(value);
            OnItemPerformed.Invoke();
        }

        protected override void UseItemActionCancelled(InputAction.CallbackContext value)
        {
            base.UseItemActionPerformed(value);
            OnItemCancelled.Invoke();
        }
    }
}