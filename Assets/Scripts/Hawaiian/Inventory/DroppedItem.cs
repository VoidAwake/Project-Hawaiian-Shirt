using System;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Hawaiian.Inventory
{
    public class DroppedItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private Item item;

        public Item Item
        {
            get => item;
            private set
            {
                item = value;

                OnItemChanged();
            }
        }

        private void Awake()
        {
            // Must be called in awake to update static items before highlighters
            OnItemChanged();
        }

        public void Initialise(Item item)
        {
            // Must be called in initialise to update dynamic items before highlighters
            Item = item;
        }
        
        private void Start()
        {
            // Must be called in start to validate all items 
            ValidateItem();
        }

        public void OnPickUp()
        {
            Destroy(gameObject);
        }

        public void OnItemChanged()
        {
            if (Item == null) return;
            
            name = Item.name + " Item";

            if (spriteRenderer == null) return;

            spriteRenderer.sprite = Item.DroppedItemSprite;

            spriteRenderer.color = Item.IsDetonator ? Color.red : Color.white;
        }

        private void ValidateItem()
        {
            if (Item != null) return;
            
            Debug.LogWarning($"{nameof(DroppedItem)} has not been assigned an {nameof(Item)}. Disabling.");
                
            gameObject.SetActive(false);
        }
    }
}