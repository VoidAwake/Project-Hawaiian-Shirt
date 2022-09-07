using System.Collections.Generic;
using System.Linq;
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

        private List<HeldItemBehaviour> heldItemBehaviours = new();

        private void OnEnable()
        {
            inventoryController.inventoryChanged.AddListener(OnItemUpdated);
        }

        private void OnDisable()
        {
            inventoryController.inventoryChanged.AddListener(OnItemUpdated);
        }

        private void OnItemUpdated()
        {
            if (heldItemObject != null)
            {
                DestroyHeldItem();
            }
            
            // TODO: Should just be part of the held item
            heldItemSpriteRenderer.sprite = Item != null ? Item.ItemSprite : null;

            if (Item == null) return;

            if (Item.heldItemPrefab == null) return;
            
            CreateHeldItem(Item);
        }

        private void CreateHeldItem(Item currentItem)
        {
            heldItemObject = Instantiate(currentItem.heldItemPrefab, transform);
            
            // TODO: Two way dependency.
            heldItemBehaviours = heldItemObject.GetComponents<HeldItemBehaviour>().ToList();

            foreach (var heldItemBehaviour in heldItemBehaviours)
            {
                heldItemBehaviour.Initialise(this);

                heldItemBehaviour.destroyed.AddListener(OnHeldItemDestroyed);
            }
        }

        private void DestroyHeldItem()
        {
            foreach (var heldItemBehaviour in heldItemBehaviours)
            {
                heldItemBehaviour.destroyed.RemoveListener(OnHeldItemDestroyed);
            }

            Destroy(heldItemObject);

            heldItemObject = null;
        }

        private void OnHeldItemDestroyed()
        {
            // TODO: Should have a more generic method that takes the item.
            inventoryController.RemoveCurrentItem();
        }
    }
}