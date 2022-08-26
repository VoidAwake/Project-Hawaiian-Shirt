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
            set
            {
                item = value;

                OnItemChanged();
            }
        }

        public void OnPickUp()
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            if (Application.isPlaying) return;

            OnItemChanged();
        }

#endif
        
        private void OnItemChanged()
        {
            if (Item == null) return;

            name = Item.name + " Item";

            if (spriteRenderer == null) return;

            spriteRenderer.sprite = Item.DroppedItemSprite;

            spriteRenderer.color = Item.IsDetonator ? Color.red : Color.white;
        }
    }
}