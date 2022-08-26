using System;
using Hawaiian.PositionalEvents;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class DetonatorHeldItem : HeldItemBehaviour
    {
        [SerializeField] private PlayerTreasure _lastInteractedTreasure;
        [SerializeField] private GameObject _detonatorPrefab;
        
        private bool _canPlaceDetonator = true;

        public void OnEnable()
        {
            _canPlaceDetonator = false;
        }

        public void BeginDetonation() => Instantiate(_detonatorPrefab, transform.position, Quaternion.identity);


        protected override void UseItemActionPerformed(InputAction.CallbackContext value)
        {
            if (!_canPlaceDetonator)
                return;

            base.UseItemActionPerformed(value);

            BeginDetonation();
            _canPlaceDetonator = false; // ensures that if the detonator item updates even if it is not properly removed
            Debug.Log("Detonation began");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            PlayerTreasure treasure = col.gameObject.GetComponent<PlayerTreasure>();

            if (treasure == null)
                return;

            _lastInteractedTreasure = treasure;
            UnitPlayer owner = treasure.Owner;

            if (owner == null)
                return;

            _canPlaceDetonator = owner != itemHolder.unitPlayer && _lastInteractedTreasure.CanBeDetonated();
            Debug.Log(_canPlaceDetonator);
        }


        private void OnTriggerExit2D(Collider2D col)
        {
            _canPlaceDetonator = false;
        }
    }
}