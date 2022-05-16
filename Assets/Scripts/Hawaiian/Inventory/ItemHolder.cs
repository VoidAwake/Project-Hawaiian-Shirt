using System;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] private InventoryController inventoryController;

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
                Destroy(heldItem.gameObject);

                heldItem = null;
            }
            
            var currentItem = inventoryController.CurrentItem;

            if (currentItem == null) return;

            if (currentItem.heldItemPrefab == null) return;
            
            var heldItemObject = Instantiate(currentItem.heldItemPrefab, transform);

            heldItem = heldItemObject.GetComponent<HeldItem>();
        }

        // Message from Player Input
        private void OnAttack()
        {
            if (heldItem == null) return;
            
            heldItem.Use();
        }
    }
}