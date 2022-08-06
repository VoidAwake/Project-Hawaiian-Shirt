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
        [SerializeField] public SpriteRenderer heldItemSpriteRenderer;

        public Item Item => inventoryController.CurrentItem;

        private GameObject heldItemObject;

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
            if (heldItemObject != null)
            {
                DestroyHeldItem();
            }
            
            // TODO: Is should just be part of the held item
            heldItemSpriteRenderer.sprite = Item != null ? Item.ItemSprite : null;

            if (Item == null) return;

            if (Item.heldItemPrefab == null) return;
            
            CreateHeldItem(Item);
        }

        private void CreateHeldItem(Item currentItem)
        {
            heldItemObject = Instantiate(currentItem.heldItemPrefab, transform);
            
            // TODO: Two way dependency.
            var heldItemBehaviours = heldItemObject.GetComponents<HeldItemBehaviour>();

            foreach (var heldItemBehaviour in heldItemBehaviours)
            {
                heldItemBehaviour.Initialise(this);
            }

            // TODO: Come back to this
            // heldItem.destroyed.AddListener(OnHeldItemDestroyed);
        }

        private void DestroyHeldItem()
        {
            // heldItem.destroyed.RemoveListener(OnHeldItemDestroyed);

            Destroy(heldItemObject);

            heldItemObject = null;
        }

        private void OnHeldItemDestroyed()
        {
            inventoryController.RemoveCurrentItem();
        }
    }
}