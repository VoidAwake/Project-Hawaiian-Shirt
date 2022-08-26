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
        [SerializeField] private GameObject _detonatorPrefab;

        private bool _canPlaceDetonator = true;
       [SerializeField] private PlayerTreasure _lastInteractedTreasure;

        public void OnEnable()
        {
            _canPlaceDetonator = false;
        }

        public void BeginDetonation() => Instantiate(_detonatorPrefab, transform.position, Quaternion.identity);
        
     
        protected override void UseItemActionPerformed(InputAction.CallbackContext value)
        {
            Debug.Log("Can be detonated 1: " + _lastInteractedTreasure.CanBeDetonated());

            if (!_canPlaceDetonator)
                return;
            
            Debug.Log("Can be detonated 2: " + _lastInteractedTreasure.CanBeDetonated());
            
            base.UseItemActionPerformed(value);
            
            BeginDetonation();
            Debug.Log("Detonation began");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            
            PlayerTreasure treasure = col.gameObject.GetComponent<PlayerTreasure>();

            if (treasure == null)
                return;

            _lastInteractedTreasure = treasure;
            UnitPlayer owner = treasure.Owner;

            if ( owner == null)
                return;
                
            _canPlaceDetonator = owner != itemHolder.unitPlayer && _lastInteractedTreasure.CanBeDetonated() ;
            Debug.Log(_canPlaceDetonator);
           
        }   
        
     
        
        private void OnTriggerExit2D(Collider2D col)
        {
            _canPlaceDetonator = false;
        }
    }
}