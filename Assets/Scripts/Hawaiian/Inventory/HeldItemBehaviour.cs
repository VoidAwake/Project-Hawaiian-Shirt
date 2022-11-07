using System;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory
{
    public class HeldItemBehaviour : MonoBehaviour
    {
        public Item Item => itemHolder.Item;
        protected ItemHolder itemHolder;
        public UnitPlayer UnitPlayer => itemHolder.unitPlayer;
        protected Cursor Cursor => itemHolder.cursor;
        
        public bool UseItemActionHeld { get; private set; }
        
        protected virtual void OnDestroy()
        {
            UnitPlayer.GetPlayerInput().actions["Attack"].performed -= UseItemActionPerformed; 
            UnitPlayer.GetPlayerInput().actions["Attack"].canceled -= UseItemActionCancelled; 
        }

        // TODO: This feels like the right way, but it doesn't work.
        // protected virtual void OnAttack(InputAction.CallbackContext value)
        // {
        //     if (value.performed)
        //         UseItemActionPerformed(value);
        //     
        //     if (value.canceled)
        //         UseItemActionCancelled(value);
        // }

        protected virtual void UseItemActionPerformed(InputAction.CallbackContext value)
        {
            if (!value.performed)
                return;

            UseItemActionHeld = true;
        }

        protected virtual void UseItemActionCancelled(InputAction.CallbackContext value)
        {
            if (value.performed)
                return;

            UseItemActionHeld = false;
        }

        public virtual void Initialise(ItemHolder itemHolder)
        {
            this.itemHolder = itemHolder;
            
            UnitPlayer.GetPlayerInput().actions["Attack"].performed += UseItemActionPerformed; 
            UnitPlayer.GetPlayerInput().actions["Attack"].canceled += UseItemActionCancelled; 
        }
    }
}