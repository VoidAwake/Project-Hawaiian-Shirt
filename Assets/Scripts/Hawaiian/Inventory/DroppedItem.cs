using System;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class DroppedItem : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private Item _item;

        private SpriteRenderer _renderer;

        public Item Item => _item;
        
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void UpdateItem(Item newItem)
        {
            _item = newItem;
            UpdateItemSprite();
        }
        public void UpdateItemSprite() => _renderer.sprite = _item.DroppedItemSprite;
        
    }
}

        public void OnPickUp()
        {
        private void OnValidate()
        }
            Destroy(gameObject);

        {
            if (item == null) return;

            name = item.name + " Item";
            if (spriteRenderer == null) return;


            spriteRenderer.sprite = item.itemSprite;
        }
}
    }