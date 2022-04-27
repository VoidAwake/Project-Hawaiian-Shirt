using Hawaiian.PositionalEvents;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private GameEvent parse;
        [SerializeField] private bool addinv;
        [SerializeField] private Item item;

        [SerializeField] private BaseGameEvent<Inventory> addedInventory;

        [SerializeField] private SpriteRenderer hand;
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

        private void Update()
        {
            if (!addinv) return;
            
            _inv.PickUp(item);
            addinv = !addinv;
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
                hand.sprite = _inv.inv[_inv.invPosition].itemSprite;
            }
            else
            {
                hand.sprite = null;
            }
            
            //how do i call an event c:
            parse.Raise();
            
        }

        private void SelectionUpdate()
        {
            
        }
    }
}
