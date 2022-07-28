using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] private InventoryController inventoryController;
        
        // TODO: Need to reconsider these references
        [SerializeField] public UnitPlayer unitPlayer;
        [SerializeField] public Cursor cursor;
        [SerializeField] public Transform firePoint;

        private HeldItem heldItem;

        private void OnEnable()
        {
            inventoryController.currentItemChanged.AddListener(OnItemUpdated);
        }

        private void OnDisable()
        {
            inventoryController.currentItemChanged.AddListener(OnItemUpdated);
        }

        private void OnItemUpdated()
        {
            if (heldItem != null)
            {
                DestroyHeldItem();
            }
            
            var currentItem = inventoryController.CurrentItem;

            if (currentItem == null) return;

            if (currentItem.heldItemPrefab == null) return;
            
            CreateHeldItem(currentItem);
        }

        private void CreateHeldItem(Item currentItem)
        {
            var heldItemObject = Instantiate(currentItem.heldItemPrefab, transform);

            heldItem = heldItemObject.GetComponent<HeldItem>();

            heldItem.destroyed.AddListener(OnHeldItemDestroyed);
        }

        private void DestroyHeldItem()
        {
            heldItem.destroyed.RemoveListener(OnHeldItemDestroyed);

            Destroy(heldItem.gameObject);

            heldItem = null;
        }

        private void OnHeldItemDestroyed()
        {
            inventoryController.RemoveCurrentItem();
        }

        // Message from Player Input
        private void OnAttack()
        {
            if (heldItem == null) return;
            
            heldItem.Use();
        }
    }
}