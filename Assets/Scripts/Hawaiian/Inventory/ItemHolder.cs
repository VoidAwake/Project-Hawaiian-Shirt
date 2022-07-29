using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

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
            
            // TODO: Is should just be part of the held item
            heldItemSpriteRenderer.sprite = currentItem.ItemSprite;

            if (currentItem == null) return;

            if (currentItem.heldItemPrefab == null) return;
            
            CreateHeldItem(currentItem);
        }

        private void CreateHeldItem(Item currentItem)
        {
            var heldItemObject = Instantiate(currentItem.heldItemPrefab, transform);

            heldItem = heldItemObject.GetComponent<HeldItem>();

            heldItem.Initialise(currentItem);

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
        private void OnAttack(InputValue value)
        {
            if (heldItem == null) return;

            if (value.Get<float>() > 0.5)
            {
                heldItem.UseDown();
            }
            else
            {
                heldItem.Use();
            }
        }
    }
}