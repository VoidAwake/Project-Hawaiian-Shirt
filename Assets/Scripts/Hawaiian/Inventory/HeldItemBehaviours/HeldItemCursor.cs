using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class HeldItemCursor : HeldItemBehaviour
    {
        private float _currentHoldTime;
        private Vector2 _rotation;
        private bool _isJoystickNeutral = true;

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
                    _currentHoldTime += Time.deltaTime;
                    UpdateHoldAttackCursor();
                }
            }
        }
        
        public void OnRotate(InputValue value)
        {
            _rotation = value.Get<Vector2>();

            if (_rotation == Vector2.zero) // idk why rider being baby but seems to work fine
            {
                _isJoystickNeutral = true;
                return;
            }

            _isJoystickNeutral = false;
        }

        public void OnMoveCursor(InputValue value)
        {
            _rotation = value.Get<Vector2>();
        }


        protected override void UseItemActionCancelled(InputAction.CallbackContext value)
        {
            base.UseItemActionCancelled(value);
            
            Cursor.LerpToReset();
            _currentHoldTime = 0f;
        }
        
        void UpdateHoldAttackCursor() => Cursor.CurrentRad += _currentHoldTime / 0.5f;

        void CancelRotation()
        {
            _rotation = Vector2.zero;
            _isJoystickNeutral = true;
        }
    }
}