using System;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class DroppedItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

       [SerializeField] private Item _item;
        public Item item
        {
            get => _item;
            set
            {
                _item = value;
                
                if (spriteRenderer == null)
                    return;
                
                spriteRenderer.sprite = item.ItemSprite;
            }
        }

        public void OnPickUp()
        {
            Destroy(gameObject);
        }
        
        private void OnValidate()
        {
            if (item == null) return;

            name = item.name + " Item";

            if (spriteRenderer == null) return;

            spriteRenderer.sprite = item.ItemSprite;
        }
    }
}
