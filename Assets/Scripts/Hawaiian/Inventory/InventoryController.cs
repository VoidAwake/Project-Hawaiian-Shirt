using Hawaiian.PositionalEvents;
using Hawaiian.Utilities;
using UnityEngine;


namespace Hawaiian.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private bool addinv;
        [SerializeField] private Item item;

        [SerializeField] private BaseGameEvent<Inventory> addedInventory;
        //[SerializeField] private int invSize;
        

        private Inventory _inv;
        private PositionalEventCaller positionalEventCaller;
    
        private void Awake()
        {
            _inv = ScriptableObject.CreateInstance<Inventory>();
            
            addedInventory.Raise(_inv);

            addinv = false;

            positionalEventCaller = GetComponent<PositionalEventCaller>();
        }

        public void InitialiseHighlight()
        {
            if (!addinv) return;
            
            _inv.PickUp(item);
            addinv = !addinv;
        }
        
        void UpdateRotation(Vector2 newValue)
        {
            _rotation = newValue;
            _isJoystickNeutral = false;
        }

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

        private void Parse(int i)
        {
            invPosition += i;
            if (invPosition > _inv.inv.Length - 1)
            {
                invPosition = 0;
            }

            if (invPosition < 0)
            {
                invPosition = _inv.inv.Length - 1;
            }
            SelectionUpdate();
        }


        public void UpdateItem()
        {
            _projectileReference = GetCurrentItem().ProjectileInstance;
            handheld.sprite = GetCurrentItem().ItemSprite;
            _cursor.MaxRadius = GetCurrentItem().DrawDistance;
        }

        public void SelectionUpdate()
        {
            highRef.transform.position = tempRef.invSlots[invPosition].transform.position;
            handheld.sprite = _inv.inv[invPosition] != null ? _inv.inv[invPosition].ItemSprite : null;
            
            UpdateItem();
        }
    }
}
