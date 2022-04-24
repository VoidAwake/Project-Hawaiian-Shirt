using Hawaiian.PositionalEvents;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hawaiian.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private GameObject UI;
        [FormerlySerializedAs("canvas")] [SerializeField] private Transform uiParent;
        [SerializeField] private bool addinv;
        [SerializeField] private Item item;
        //[SerializeField] private int invSize;
        
        private GameObject reference;
        private Inventory _inv;
        private PositionalEventCaller positionalEventCaller;
    
        private void Awake()
        {
            // TODO: Temp fix to get canvas reference. Will come back to this when we start looking at a UI system.
            if (uiParent == null) uiParent = FindObjectOfType<Canvas>().transform.GetChild(0);
        
            _inv = ScriptableObject.CreateInstance<Inventory>();
            reference = Instantiate(UI, uiParent);
            reference.GetComponent<InventoryUI>().inv = _inv;
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
                var item = target.GetComponent<tempItem>().item;
                
                if (item == null) continue;

                if (!_inv.PickUp(item)) continue;
                
                positionalEventCaller.Raise(target);
            }
        }
    }
}
