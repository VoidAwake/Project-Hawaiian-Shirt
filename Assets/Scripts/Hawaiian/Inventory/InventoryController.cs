using UnityEngine;
using UnityEngine.Serialization;

namespace Hawaiian.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private GameObject UI;
        [FormerlySerializedAs("canvas")] [SerializeField] private Transform uiParent;
        private GameObject reference;
        private Inventory _inv;
        [SerializeField] private bool addinv;
        [SerializeField] private Item item;
        //[SerializeField] private int invSize;
    
        private void Awake()
        {
            // TODO: Temp fix to get canvas reference. Will come back to this when we start looking at a UI system.
            if (uiParent == null) uiParent = FindObjectOfType<Canvas>().transform.GetChild(0);
        
            _inv = ScriptableObject.CreateInstance<Inventory>();
            reference = Instantiate(UI, uiParent);
            reference.GetComponent<InventoryUI>().inv = _inv;
            addinv = false;
        }

        private void Update()
        {
            if (addinv)
            {
                _inv.PickUp(item);
                addinv = !addinv;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Item")
            {
                if (_inv.PickUp(col.gameObject.GetComponent<tempItem>().item))
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }
}
