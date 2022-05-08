using System;
using Hawaiian.PositionalEvents;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Hawaiian.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UnitPlayer _player;
        [SerializeField] private GameEvent parse;
        [SerializeField] private bool addinv;
        [SerializeField] private Item item;
        [SerializeField] private ScriptableInt size;

        [SerializeField] private BaseGameEvent<Inventory> addedInventory;

        [SerializeField] private SpriteRenderer hand;
        
        //[SerializeField] private int invSize;a
        

        private Inventory _inv;
        private PositionalEventCaller positionalEventCaller;

        public Item GetCurrentItem() => _inv.inv[_inv.invPosition];
    
        private void Awake()
        {
            _inv = ScriptableObject.CreateInstance<Inventory>();
            _inv.SetInventory(size.Value);
            
            addedInventory.Raise(_inv);
            _player = GetComponentInParent<UnitPlayer>();
          
            addinv = false;
            positionalEventCaller = GetComponent<PositionalEventCaller>();
        }

       
        

        private void Start()
        {
            _player.GetPlayerInput().actions["InvParse"].performed += SwitchItem;
        }


        private void OnDisable()
        {
            _player.GetPlayerInput().actions["InvParse"].performed -= SwitchItem;
        }

        public void InitialiseHighlight()
        {
            if (!addinv) return;
            
            _inv.PickUp(item);
            addinv = !addinv;
        }
        //
        // void UpdateRotation(Vector2 newValue)
        // {
        //     _rotation = newValue;
        //     _isJoystickNeutral = false;
        // }

        private void OnPickUp()
        {
            foreach (var target in positionalEventCaller.Targets)
            {
                var item = target.GetComponent<DroppedItem>().item;
                
                if (item == null) continue;

                if (!_inv.PickUp(item)) continue;
                
                GetComponent<ItemInteractor>().UpdateItem();

                positionalEventCaller.Raise(target);
                
            }
        }


        public void SwitchItem(InputAction.CallbackContext value)
        {
            if (!value.performed)
                return;
            
            if (value.ReadValue<float>() > 0f)
                OnCycleBackward();
            else
                OnCycleForward();

            GetComponent<ItemInteractor>().UpdateItem();

        }
        public void OnCycleForward()
        {
            _inv.invPosition++;
            Parse();
        }

        public void OnCycleBackward()
        {
            _inv.invPosition--;
            Parse();
        }
        private void Parse()
        {
             //_inv.invPosition += i;
            if (_inv.invPosition > _inv.inv.Length - 1)
            {
                _inv.invPosition = 0;
            }

            if (_inv.invPosition < 0)
            {
                _inv.invPosition = _inv.inv.Length - 1;
            }
            //SelectionUpdate();
            if (_inv.inv[_inv.invPosition] != null)
            {
                hand.sprite = _inv.inv[_inv.invPosition].ItemSprite;
            }
            else
            {
                hand.sprite = null;
            }
            
            //how do i call an event c:
            
       
            
            parse.Raise();
            
        }

        public void UseItem()
        {
            
        }

        private void SelectionUpdate()
        {
            
        }
    }
}
