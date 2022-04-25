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
    }
}
