using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class HeldItemCursor : HeldItemBehaviour
    {
        private float currentHoldTime;

        protected override void Initialise(ItemHolder itemHolder)
        {
            base.Initialise(itemHolder);
            
            Cursor.MaxRadius = Item.DrawDistance;
        }

        private void FixedUpdate()
        {
            if (UseItemActionHeld)
            {
                if (Math.Abs(Cursor.CurrentRad - Cursor.MaxRadius) > 0.01f)
                {
                    currentHoldTime += Time.deltaTime;
                    UpdateHoldAttackCursor();
                }
            }
        }

        protected override void UseItemActionCancelled(InputAction.CallbackContext value)
        {
            base.UseItemActionCancelled(value);
            
            Cursor.LerpToReset();
            currentHoldTime = 0f;
        }
        
        void UpdateHoldAttackCursor() => Cursor.CurrentRad += currentHoldTime / 0.5f;
    }
}