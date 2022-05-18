using Hawaiian.PositionalEvents;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;


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

        [SerializeField] private GameObject droppedItem;

        public UnityEvent currentItemChanged = new UnityEvent();

       
        //[SerializeField] private int invSize;a


        public Inventory _inv;
        private PositionalEventCaller positionalEventCaller;

        public Item CurrentItem => _inv.CurrentItem;
    
        private void Awake()
        {
            _inv = ScriptableObject.CreateInstance<Inventory>();
            _inv.SetInventory(size.Value);
            _inv.currentItemChanged.AddListener(OnCurrentItemChanged);
            
            addedInventory.Raise(_inv);
            _player = GetComponentInParent<UnitPlayer>();
          
            addinv = false;
            positionalEventCaller = GetComponent<PositionalEventCaller>();
        }

        private void OnCurrentItemChanged()
        {
            currentItemChanged.Invoke();
        }

        // TODO: Replace with messages
        private void OnEnable()
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
        }
        
        public void OnCycleForward()
        {
            // TODO: Two way dependency.
            // TODO: Replace with a cancel attack function
            if (GetComponent<ItemInteractor>().IsAttacking) // makes sure that they cant change their items while attacking since that make it go brokey
                return;
            
            _inv.InvPosition++;
            Parse();
        }

        public void OnCycleBackward()
        {
            // TODO: Two way dependency.
            // TODO: Replace with a cancel attack function
            if (GetComponent<ItemInteractor>().IsAttacking) // makes sure that they cant change their items while attacking since that make it go brokey
                return;
            
            _inv.InvPosition--;
            Parse();
        }
        
        private void Parse()
        {
            
             //_inv.invPosition += i;
            if (_inv.InvPosition > _inv.inv.Length - 1)
            {
                _inv.InvPosition = 0;
            }

            if (_inv.InvPosition < 0)
            {
                _inv.InvPosition = _inv.inv.Length - 1;
            }
            //SelectionUpdate();
            if (_inv.inv[_inv.InvPosition] != null)
            {
                hand.sprite = _inv.inv[_inv.InvPosition].ItemSprite;
                
            }
            else
            {
                hand.sprite = null;
            }
            
            //how do i call an event c:



            parse.Raise();
            
        }
        
        public void OnDrop()
        {
            DropItem(_inv.InvPosition);
        }

        public void RemoveCurrentItem()
        {
            RemoveItemFromIndex(_inv.InvPosition);
        }


        public void DropRandom()
        {
            var itemIndexes = new List<int>();

            for (int i = 0; i < _inv.inv.Length; i++)
            {
                if (_inv.inv[i] != null)
                    itemIndexes.Add(i);
            }

            if (itemIndexes.Count == 0) return;

            var randomItemIndex = itemIndexes[UnityEngine.Random.Range(0, itemIndexes.Count)];

            DropItem(randomItemIndex);
        }

        private void DropItem(int invPosition)
        {
            if (_inv.inv[invPosition] == null) return;
            
            GameObject dp = Instantiate(droppedItem, transform.position, quaternion.identity);
            dp.GetComponent<DroppedItem>().item = _inv.inv[invPosition];
            dp.GetComponent<SpriteRenderer>().sprite = _inv.inv[invPosition].DroppedItemSprite;
            RemoveItemFromIndex(invPosition);
        }

        public void RemoveItemFromIndex(int invPosition)
        {
            if (_inv.inv[invPosition] == null) return;
            
            _inv.DropItem(_inv.InvPosition);
            hand.sprite = null;
        }

        public void UseItem()
        {
            
        }

        private void SelectionUpdate()
        {
            
        }
    }
}
